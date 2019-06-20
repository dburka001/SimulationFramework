using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.Entities
{
    public class FertilityEduMultiplierEntity : IPopulationCompleteEntity
    {
        public int Year { get; set; }
        public int Age { get; set; }
        public Gender Gender { get; set; }        
        public Education Education { get; set; }
        public BirthOrder BirthOrder { get; set; }
        public decimal? Value { get; set; }
    }
}
