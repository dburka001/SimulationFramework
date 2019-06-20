using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.CohortModel.Entities
{
    public class PopulationEntity
    {
        public int Year { get; set; }
        public int Gender { get; set; }
        public int BirthYear { get; set; }
        public decimal Population { get; set; }
        public decimal Deaths { get; set; }
        public decimal Births { get; set; }
    }
}
