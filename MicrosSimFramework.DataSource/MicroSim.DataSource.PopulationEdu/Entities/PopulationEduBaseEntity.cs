using MicroSim.DataSource.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.Entities
{
    public class PopulationEduBaseEntity : IGenderEntity, IEducationEntity, IYearEntity, IValueEntity
    {        
        public Gender Gender { get; set; }
        public Education Education { get; set; }
        public int AgeStart { get; set; }
        public int AgeEnd { get; set; }        
        public int Year { get; set; }
        public decimal? Value { get; set; }
    }
}
