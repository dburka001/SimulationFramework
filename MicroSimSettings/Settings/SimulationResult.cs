using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        public object Value { get; set; }        

        public T GetValue<T>(string valueName)
        {
            Type type = Value.GetType();
            T itemvalue = (T)type.GetProperty(valueName).GetValue(Value, null);
            return itemvalue;
        }

        public bool CheckValue<T>(string valueName)
        {
            Type type = Value.GetType();
            PropertyInfo property = type.GetProperty(valueName);
            if (property == null)
                return false;
            else
                return property.GetType() == typeof(T);               
        }
    }    
}
