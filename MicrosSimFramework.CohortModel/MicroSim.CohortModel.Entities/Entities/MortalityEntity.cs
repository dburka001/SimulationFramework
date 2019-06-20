using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.CohortModel.Entities
{
    public class MortalityEntity
    {
        public int Year { get; set; }
        public int Age { get; set; }
        public int Gender { get; set; }        
        public decimal Value { get; set; }        
    }
}
