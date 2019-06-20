using MicroSim.DataSource.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.Entities
{
    public class DetEduEntity : IGenderEntity, IValueEntity
    {
        public DetEducationType Type { get; set; }
        public Gender Gender { get; set; }
        public Education MotherEducation { get; set; }
        public Education ChildEducation { get; set; }
        public decimal? Value { get; set; }
    }
}
