using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MicroSimSettings
{
    public class ModelData
    {
        public static ModelData Instance { get; set; } = new ModelData();

        // Containers
        public ModelDataTable PopulationData { get; set; }
        public SimulationEnvironment Parameters { get; set; }

        // Results
        public List<SimulationResult> FinishedSimulations { get; set; }
        public Dictionary<AgeTreeInput, string> AgeTreeResults { get; set; }

        public ModelData()
        {
            if (ModelData.Instance != null) throw new NotImplementedException();
            ResetToDefault();
        }

        public void ResetToDefault()
        {
            this.FinishedSimulations = new List<SimulationResult>();
            this.AgeTreeResults = new Dictionary<AgeTreeInput, string>();
            this.PopulationData = null;
            this.Parameters = null;
        }

        public void AddSimulationResult(SimulationResult sr)
        {
            FinishedSimulations.Add(sr);
            addAgeTreeResults(sr);
        }

        private void addAgeTreeResults(SimulationResult sr)
        {
            string key = "";
            if (sr.Params.Count > 0)
            {
                key += " { ";
                key += sr.Params.Select(w => w.Key + ": " + w.Value).Aggregate((c, n) => c + ", " + n);
                key += " }";
            }
            foreach (Result r in sr.Results)
            {
                AgeTreeInput currentAgeTree = isAgeTree(r.Values);
                if (currentAgeTree != null)
                {
                    AgeTreeResults.Add(currentAgeTree, r.Name + key);
                }
            }
        }

        private AgeTreeInput isAgeTree(List<ResultItem> data)
        {
            if (data[0].Key == null) return null; // Key required   
            Type keyType = data[0].Key.GetType();
            PropertyInfo[] keyProperties = keyType.GetProperties();
            if (keyProperties.Count() != 2) return null; // Exactly 2 properties are required            
            if (data.Select(x => keyProperties[0].GetValue(x.Key)).Distinct().Count() != 2) return null; // Property 0 should be gender
            if (data.FirstOrDefault().CheckValue<int>("Korfa")) return null;
            List<int> years = data.Select(x => x.Year).Distinct().ToList<int>(); years.Sort();
            return createAgeTreeInput(years, data, keyProperties);
        }

        private AgeTreeInput createAgeTreeInput(List<int> years, List<ResultItem> data, PropertyInfo[] keyProperties)
        {
            double xMax = 0, yMax = 0;
            List<string> inputData = new List<string>();
            //List<object> genders = data.Select(x => keyProperties[0].GetValue(x.Key)).Distinct().ToList<object>(); genders.Sort();
            //List<object> groups = data.Select(x => keyProperties[1].GetValue(x.Key)).Distinct().ToList<object>(); groups.Sort();
            
            var groupedDataByYear = data.AsParallel().GroupBy(x => x.Year).OrderBy(group => group.Key);
            foreach (var groupByYear in groupedDataByYear)
            {
                string yearString = "[";
                var groupedData = groupByYear.AsParallel()
                                             .GroupBy(x => keyProperties[1].GetValue(x.Key))
                                             .OrderBy(group => group.Key);
                yMax = groupedData.Count();
                foreach (var currentGroup in groupedData)
                {                     
                    string groupString = "{ group: '" + currentGroup.Key + "' ";                    
                    bool isMale = true;
                    foreach (var item in currentGroup.OrderBy(p => keyProperties[0].GetValue(p.Key)))
                    {                        
                        if (isMale)
                        {
                            isMale = false;
                            groupString += ", male: ";
                        }
                        else groupString += ", female: ";
                        var currentValue = item.GetValue<int>("Korfa");
                        groupString += currentValue.ToString();                        
                        if (currentValue > xMax) xMax = currentValue;
                        // groupString += (item.Value * 210.0 / 1000.0).ToString();
                        // if (item.Value * 210.0 / 1000.0 > xMax) xMax = (item.Value * 210.0 / 1000.0);
                    }
                    groupString += "}";
                    if (currentGroup != groupedData.Last()) groupString += ", ";
                    yearString += groupString;
                }
                yearString += "]";                
                inputData.Add(yearString);
            }
            return new AgeTreeInput(years.ToArray(), inputData.ToArray(), xMax, yMax);
        }

        // TODO load errors
        public void LoadPopulationData()
        {
            this.PopulationData = new ModelDataTable(ModelSettings.Instance.PopulationDataFileName);
        }

        public void LoadParameters()
        {
            this.Parameters = new SimulationEnvironment();
            ExcelParser.Parse(ModelSettings.Instance.ParametersFileName, this.Parameters);
        }
    }

    public class AgeTreeInput
    {
        public int[] Years { get; set; }
        public double yMax { get; set; }
        public double xMax { get; set; }
        public string[] InputData { get; set; }

        public AgeTreeInput(int[] years, string[] inputData, double xmax, double ymax)
        {
            this.Years = years;
            this.InputData = inputData;
            this.yMax = ymax;
            this.xMax = xmax;
        }
    }
}
