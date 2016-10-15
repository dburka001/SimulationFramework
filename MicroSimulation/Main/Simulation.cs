using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicroSimSettings;
using System.Threading;
using System.Collections;
using System.Collections.Concurrent;
using System.Reflection;

namespace MicroSimulation
{    
    public partial class Simulation : IDisposable 
    {
        public ModelSettings Settings;
        public CancellationTokenSource CancelTokenSource { get; set; }
        private List<Result> Results;
        private static Random GlobalRng;
        private ParallelOptions p = new ParallelOptions();        
        // private TaskScheduler ui = TaskScheduler.FromCurrentSynchronizationContext();
        private Constant[] multiValueContstants;
        private int currentSimulation;
        private int nbrOfSimulations = 1;

        public event SimulationStarted SimulationStarted;
        public event YearFinished YearFinished;
        public event SimulationFinished SimulationFinished;
        public event SimulationFinishedCompletely SimulationFinishedCompletely;
        public event SimulationFinishedCompletely Canceled;

        public Simulation()
        {
            this.Settings = ModelSettings.Instance;
            this.p.MaxDegreeOfParallelism = 8;
            this.CancelTokenSource = new CancellationTokenSource();            
        }

        public void Run()
        {
            SimulationStarted(this, new EventArgs());
            ClearMemory();
            foreach (Constant c in Settings.Constants) c.CurrentValue = c.From;            
            multiValueContstants = (from x in Settings.Constants where x.IsMultipleValue select x).ToArray<Constant>();
            foreach (Constant c in multiValueContstants)
            {
                nbrOfSimulations *= 1 + (int)Math.Floor((c.To - c.From) / c.Step);
            }
            currentSimulation = 0;
            runSimulation();
        }

        public void Cancel()
        {
            this.CancelTokenSource.Cancel();
            Canceled(this, new EventArgs());
        }

        public void ClearMemory()
        {
            if (Population != null) Population.Clear();
            if (Households != null) Households.Clear();
            GC.Collect();
        }

        private void runSimulation()
        {
            currentSimulation += 1;
            GlobalRng = new Random(1234);
            this.Results = new List<Result>();
            createCompiledClasses();
            var compute = Task.Factory.StartNew(() => { initiatePopulation(); }, CancelTokenSource.Token);
            compute.ContinueWith(resultTask => { runSimulationNextYear(Settings.StartYear); }, TaskContinuationOptions.NotOnCanceled);
        }

        private void runSimulationNextYear(int year)
        {
            int currentYear = year;
            var resultNewBornCollection = new ConcurrentBag<object>();
            var resultNewRelationshipCollection = new ConcurrentBag<Relationship>();
            var resultNewHouseholdCollection = new ConcurrentBag<object>();

            var compute = Task.Factory.StartNew(() =>
            {
                int[] iterationRandom = new int[Population.Count];
                for (int i = 0; i < iterationRandom.Length; i++)
                {
                    iterationRandom[i] = GlobalRng.Next();
                }

                try
                {
                    Parallel.For(0, Population.Count, p, (i) =>
                    {
                        int j = i;
                        var currentPerson = Population[i];
                        int currentSeed = iterationRandom[i];
                        CancelTokenSource.Token.ThrowIfCancellationRequested();
                        SimStepOutput output = SimStep(j, currentPerson, year, currentSeed);
                        foreach (object item in output.NewBorns) { resultNewBornCollection.Add(item); }
                        if (Settings.UseHouseholds)
                        {
                            if (output.NewHousehold != null) resultNewHouseholdCollection.Add(output.NewHousehold);
                            if (output.NewRelationship != null) resultNewRelationshipCollection.Add(output.NewRelationship);                            
                        }
                    });
                }
                catch (AggregateException ex)
                {
                    foreach (Exception e in ex.InnerExceptions)
                    {
                        if (e.GetType() != typeof(OperationCanceledException)) throw e;
                    }
                }
                CancelTokenSource.Token.ThrowIfCancellationRequested();
            }, CancelTokenSource.Token);

            // Continue to next year
            compute.ContinueWith(resultTask => 
            {                
                // Households
                if (Settings.UseHouseholds)
                {                                       
                    CreateNewRelationships(
                        Population, 
                        resultNewRelationshipCollection.AsParallel().WithDegreeOfParallelism(p.MaxDegreeOfParallelism).OrderBy(o => o.Id).ToList<Relationship>(), 
                        resultNewHouseholdCollection, p, GlobalRng); // RelationShips
                    Households.RemoveAll(x => (bool)GetHouseholdIsEmpty(x));
                    /*
                    List<object> NewHouseholds = resultNewHouseholdCollection.AsParallel().WithDegreeOfParallelism(p.MaxDegreeOfParallelism)
                        .OrderBy((o) => (Household.GetProperty("ParentID").GetValue(o))).ToList<object>();
                    Households.AddRange(NewHouseholds.ToList());
                    */
                    foreach (var h in resultNewHouseholdCollection)
                    {
                        Households.Add(h);
                    }
                }                
                // NewBorns
                List<object> NewBorns = resultNewBornCollection.AsParallel().WithDegreeOfParallelism(p.MaxDegreeOfParallelism)
                    .OrderBy((o) => (Person.GetProperty("ParentID").GetValue(o))).ToList<object>();
                foreach (object item in NewBorns)
                {
                    Population.Add(item);
                }
                // Results            
                endYearResults(currentYear);
                // Next year
                if (currentYear < Settings.EndYear)
                {
                    runSimulationNextYear(currentYear + 1);
                }
                else
                {
                    // This starts if a whole simulation is finished
                    endSimulationResults();
                    for (int i = 0; i < multiValueContstants.Length; i++)
                    {
                        Constant c = multiValueContstants[i];
                        if (c.CurrentValue + c.Step <= c.To)
                        {
                            if (i > 0)
                            {
                                for (int j = 0; j < i; j++)
                                {
                                    multiValueContstants[j].CurrentValue = multiValueContstants[j].From;
                                }
                            }
                            c.CurrentValue += c.Step;
                            runSimulation();
                            break;
                        }
                    }
                    SimulationFinishedCompletely(this, new EventArgs());
                }
            }, TaskContinuationOptions.NotOnCanceled);
        }

        public string ResultsAsString()
        {
            string resultString = "";

            foreach (Result r in Results)
            {
                if (r.Values.Count == 0) continue;

                PropertyInfo[] props = new PropertyInfo[0];
                resultString += r.Name + Environment.NewLine;
                if (r.Values[0].Key != null)
                    props = r.Values[0].Key.GetType().GetProperties();
                Func<object, object>[] propGet = new Func<object, object>[props.Length];
                resultString += "Year;";
                for (int propId = 0; propId < props.Length; propId++)                
                {
                    resultString += props[propId].Name + ";";
                    propGet[propId] = createGetDelegate(props[propId]);
                }
                resultString += "Value" + Environment.NewLine;
                
                foreach (ResultItem item in r.Values)
                {
                    resultString += item.Year + ";";
                    for (int propId = 0; propId < props.Length; propId++)
                    {
                        resultString += propGet[propId](item.Key).ToString() + ";";
                    }
                    resultString += + item.Value + Environment.NewLine;
                }
            }

            return resultString;
        }

        private void endYearResults(int year)
        {
            YearFinishedEventArgs re = new YearFinishedEventArgs();            
            re.Progress = (int)Math.Floor((double)100 * (year - Settings.StartYear + 1) / (double)(Settings.EndYear - Settings.StartYear + 1));
            re.Year = year;
            GetResults.Invoke(Population, Households, Results, year);     
            if (YearFinished != null) YearFinished(this, re);
        }

        private void endSimulationResults()
        {             
            SimulationFinishedEventArgs sfe = new SimulationFinishedEventArgs();
            sfe.Progress = (int)Math.Floor((double)100 * (double)currentSimulation / (double)nbrOfSimulations);
            sfe.Results = this.Results.ToList();            
            sfe.SimParams = new Dictionary<string, double>();
            foreach (Constant c in multiValueContstants)
            {
                sfe.SimParams.Add(c.Name, c.CurrentValue);
            }
            ModelData.Instance.AddSimulationResult(new SimulationResult(sfe.SimParams, sfe.Results));
            //this.Results.Clear();
            if (SimulationFinished != null) SimulationFinished(this, sfe);            
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.
                ClearMemory();

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~Simulation() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);            
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}