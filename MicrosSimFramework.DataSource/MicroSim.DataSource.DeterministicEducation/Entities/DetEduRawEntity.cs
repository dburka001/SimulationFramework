using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.Entities
{
    public class DetEduRawEntity
    {
        public string Type { get; set; }
        public string Gender { get; set; }
        public string MotherEducation { get; set; }
        public string ChildEducation { get; set; }
        public decimal? Probability { get; set; }
    }
}
