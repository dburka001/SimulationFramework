using MicroSim.DataSource.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.Entities
{
    public class OutputFertilityEntity : IYearEntity, IAgeEntity, IEducationEntity, IValueEntity
    {
        public int Year { get; set; }
        public int Age { get; set; }
        public Education Education { get; set; }
        [MsfDisplayName(nameof(BirthOrder))]
        public byte BirthOrder { get; set; }
        public decimal? Value { get; set; }        
    }
}
