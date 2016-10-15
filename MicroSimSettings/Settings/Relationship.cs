using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSimSettings
{
    public enum ExcludeType { Equals, LowerThan, HigherThan }

    public class ExcludeTypeContainer
    {
        public string Name { get; set; }
        public ExcludeType Type { get; set; }

        public ExcludeTypeContainer(string name, ExcludeType type)
        {
            this.Name = name;
            this.Type = type;
        }
    }

    public class Relationship
    {
        public int Id { get; set; }
        public object Person { get; set; }
        public string Type { get; set; }        

        public Relationship(int id, object p, string type)
        {
            this.Id = id;
            this.Person = p;
            this.Type = type;
        }
    }

    public class RelationshipType
    {
        public string Name { get; set; }
        public string Probabilities { get; set; }
        public List<RelationshipGrouping> GroupingVariables { get; set; }

        public RelationshipType(string name, string probabilities)
        {
            this.Name = name;
            this.Probabilities = probabilities;
            this.GroupingVariables = new List<RelationshipGrouping>();
        }

        public double[] GetProbabilities()
        {            
            double[] Probabilities = Array.ConvertAll(this.Probabilities.Split(';', ','), Double.Parse);
            if (Probabilities.Length == 0) return null;
            double[] probabilites = new double[Probabilities.Length];

            // Normalize
            double sum = Probabilities.Sum();
            if (sum == 0) return null;
            for (int i = 0; i < Probabilities.Length; i++)
            {
                probabilites[i] = Probabilities[i] / sum;
            }

            // Cummulate
            for (int i = 1; i < probabilites.Length; i++)
            {
                probabilites[i] += probabilites[i - 1];
            }

            return probabilites;
        }
    }


    public class RelationshipGrouping
    {                
        public ClassField Field { get; set; }
        public double Weight { get; set; }
        public bool IsExclude { get; set; }
        public ExcludeType? ExcludeType { get; set; }
        public object ExcludeValue { get; set; }

        public RelationshipGrouping()
        {
            Weight = 1;
        }

        public RelationshipGrouping(ClassField field, bool isExclude, ExcludeType? excludeType, object excludeValue)
        {            
            Field = field;
            Weight = 1;
            IsExclude = isExclude;
            ExcludeType = excludeType;
            ExcludeValue = excludeValue;
        }
    }
}
