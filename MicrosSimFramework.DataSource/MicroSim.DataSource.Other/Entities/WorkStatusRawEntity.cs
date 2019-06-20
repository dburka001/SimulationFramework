using MicroSim.DataSource.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.Entities
{
    public class WorkStatusRawEntity : IValueEntity
    {
        public WorkStatus WorkStatusCurrent { get; set; }
        public WorkStatus WorkStatusNext { get; set; }
        public decimal? Value { get; set; }
    }
}
