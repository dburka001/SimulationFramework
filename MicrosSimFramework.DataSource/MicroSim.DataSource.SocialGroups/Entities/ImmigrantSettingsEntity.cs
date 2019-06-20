using MicroSim.DataSource.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.Entities
{
    public class ImmigrantSettingsEntity : IYearEntity, IValueEntity
    {
        public int Year { get; set; }
        public decimal? Value { get; set; }
        public decimal AgeMean { get; set; }
        public decimal AgeVariance { get; set; }
    }
}
