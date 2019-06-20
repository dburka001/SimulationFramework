using MicroSim.DataSource.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.Entities
{
    public class OutputSocialGroupEducationTypeEntity : ISocialGroupEntity
    {
        public SocialGroup SocialGroup { get; set; }
        [MsfDisplayName(nameof(DetEducationType))]
        public DetEducationType DetEducationType { get; set; }
    }
}
