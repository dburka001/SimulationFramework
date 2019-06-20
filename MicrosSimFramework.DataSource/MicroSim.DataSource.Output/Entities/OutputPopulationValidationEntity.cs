using MicroSim.DataSource.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.Entities
{
    public class OutputPopulationValidationEntity
    {       
        [MsfDisplayName(nameof(Gender))]
        public byte Gender { get; set; }
        [MsfDisplayName(nameof(BirthYear))]
        public int BirthYear { get; set; }
        [MsfDisplayName(nameof(Weight))]
        public decimal? Weight { get; set; }
    }
}
