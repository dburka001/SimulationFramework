using MicroSim.DataSource.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.Entities
{
    public class WorkStatusTimeEntity
    {
        public WorkStatus WorkStatus { get; set; }
        public decimal? MinTime { get; set; }
        public decimal? MaxTime { get; set; }
    }
}
