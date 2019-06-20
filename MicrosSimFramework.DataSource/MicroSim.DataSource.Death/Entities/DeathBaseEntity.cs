using MicroSim.DataSource.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.Entities
{
    public class DeathBaseEntity : IPopulationEntity
    {        
        public int Age { get; set; }
        public Gender Gender { get; set; }        
        public int Year { get; set; }
        public decimal? Value { get; set; }
    }
}
