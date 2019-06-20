using MicroSim.DataSource.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.Entities
{
    public class OutputPopulationEntity
    {
        [MsfDisplayName(nameof(SocialGroup))]
        public byte SocialGroup { get; set; }
        [MsfDisplayName(nameof(Gender))]
        public byte Gender { get; set; }
        [MsfDisplayName(nameof(BirthYear))]
        public int BirthYear { get; set; }
        [MsfDisplayName(nameof(Education))]
        public byte Education { get; set; }
        [MsfDisplayName(nameof(BirthOrder))]
        public byte BirthOrder { get; set; }
        [MsfDisplayName(nameof(ActiveYear))]
        public int ActiveYear { get; set; }
        [MsfDisplayName(nameof(Weight))]
        public decimal? Weight { get; set; }        
    }
}
