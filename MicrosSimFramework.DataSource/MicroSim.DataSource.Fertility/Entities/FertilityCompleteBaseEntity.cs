using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.Entities
{
    public class FertilityCompleteBaseEntity : IDemographyCompleteEntity
    {
        public int Year { get; set; }
        public int Age { get; set; }
        public Gender Gender { get; set; }        
        public Education Education { get; set; }
        public decimal? Value { get; set; }
        public decimal? Population { get; set; }
        public decimal? DeathCount { get; set; }
        public decimal? BirthCount { get; set; }
        public decimal? KtLower { get; set; }
        public decimal? Kt { get; set; }
        public decimal? KtUpper { get; set; }
        public BirthOrder BirthOrder { get; set; }
    }
}
