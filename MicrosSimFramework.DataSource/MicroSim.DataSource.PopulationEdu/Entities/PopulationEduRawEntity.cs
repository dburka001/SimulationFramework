using MicroSim.DataSource.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.Entities
{
    public class PopulationEduRawEntity : IGeoEntity, IAgeIntervalEntity, IYearEntity, IValueEntity
    {
        public string Unit { get; set; }
        public string Sex { get; set; }
        public string Education { get; set; }
        public string AgeInterval { get; set; }
        public string Geo { get; set; }
        public int Year { get; set; }
        public decimal? Value { get; set; }
    }
}
