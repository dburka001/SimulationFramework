using MicroSim.DataSource.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.Entities
{
    public class SocialGroupEducationTypeEntity : ISocialGroupEntity
    {
        public SocialGroup SocialGroup { get; set; }
        public DetEducationType DetEducationType { get; set; }
    }
}
