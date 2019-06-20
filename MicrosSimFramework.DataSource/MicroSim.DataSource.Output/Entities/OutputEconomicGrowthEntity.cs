using MicroSim.DataSource.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.Entities
{
    public class OutputEconomicGrowthEntity : IYearEntity, IValueEntity
    {
        public int Year { get; set; }
        public decimal? Value { get; set; }
    }
}
