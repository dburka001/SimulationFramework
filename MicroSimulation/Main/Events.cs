using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicroSimSettings;

namespace MicroSimulation
{
    public delegate void YearFinished(object sender, YearFinishedEventArgs e);
    public delegate void SimulationStarted(object sender, EventArgs e);
    public delegate void SimulationFinished(object sender, SimulationFinishedEventArgs e);
    public delegate void SimulationFinishedCompletely(object sender, EventArgs e);    

    public class YearFinishedEventArgs : EventArgs
    {
        public int Progress { get; set; }
        public int Year { get; set; }        
    }

    public class SimulationFinishedEventArgs : EventArgs
    {
        public int Progress { get; set; }
        public List<Result> Results { get; set; }
        public Dictionary<string, double> SimParams { get; set; }
    }
}
