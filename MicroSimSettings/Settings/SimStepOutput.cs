using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSimSettings
{
    public class SimStepOutput
    {
        public List<object> NewBorns { get; set; }
        public Relationship NewRelationship { get; set; }
        public object NewHousehold { get; set; }
    }
}
