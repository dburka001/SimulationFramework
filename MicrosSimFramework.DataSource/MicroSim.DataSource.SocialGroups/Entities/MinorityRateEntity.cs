using MicroSim.DataSource.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.Entities
{
    public class MinorityRateEntity : IAgeEntity, IGenderEntity, IEducationEntity, ISocialGroupEntity, IValueEntity
    {
        public Gender Gender { get; set; }
        public int Age { get; set; }        
        public Education Education { get; set; }
        public SocialGroup SocialGroup { get; set; }
        public decimal? Value { get; set; }        
    }
}
