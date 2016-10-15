using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSimSettings
{
    public class SimulationResult
    {
        public Dictionary<string, double> Params { get; set; }
        public List<Result> Results { get; set; }

        public SimulationResult(Dictionary<string, double> Params, List<Result> Results)
        {
            this.Params = Params;
            this.Results = Results;
        }
    }

    public class Result
    {
        public string Name { get; set; }
        public List<ResultItem> Values { get; set; }

        public Result(string name)
        {
            Name = name;
            Values = new List<ResultItem>();
        }

        public void AddSelectResult(List<ResultItem> result)
        {
            Values.AddRange(result.ToList<ResultItem>());
        }
    }

    public class ResultItem
    {
        public int Year { get; set; }
        public object Key { get; set; }
        public double Value { get; set; }
    }
}
