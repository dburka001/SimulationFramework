using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.Entities
{
    /// <summary>
    /// SocialGroup enum
    /// </summary>
    public enum SocialGroup : byte { Majority, Minority, Immigrant }

    /// <summary>
    /// SocialGroupExtensions
    /// </summary>
    public static class SocialGroupExtensions
    {
        /// <summary>
        /// Parses the specified social group.
        /// </summary>
        /// <param name="socialGroup">The social group.</param>
        /// <returns></returns>
        public static SocialGroup Parse(string socialGroup)
        {            
            switch (socialGroup)
            {
                case "0": return SocialGroup.Majority;
                case "1": return SocialGroup.Minority;
                case "2": return SocialGroup.Immigrant;               
                default: return (SocialGroup)Enum.Parse(typeof(SocialGroup), socialGroup);
            }
        }
    }
}
