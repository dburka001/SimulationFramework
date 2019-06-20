using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.Entities
{
    /// <summary>
    /// ISocialGroupEntity
    /// </summary>
    public interface ISocialGroupEntity
    {
        /// <summary>
        /// Gets or sets the social group.
        /// </summary>
        /// <value>
        /// The social group.
        /// </value>
        [MsfDisplayName(nameof(SocialGroup))]
        SocialGroup SocialGroup { get; set; }
    }
}
