using MicroSim.DataSource.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.Entities
{
    public class OutputEducationEntity : IGenderEntity, IValueEntity
    {
        [MsfDisplayName(nameof(EducationType))]
        public DetEducationType EducationType { get; set; }
        public Gender Gender { get; set; }
        [MsfDisplayName(nameof(MotherEducation))]
        public Education MotherEducation { get; set; }
        [MsfDisplayName(nameof(ChildEducation))]
        public Education ChildEducation { get; set; }
        public decimal? Value { get; set; }
    }
}
