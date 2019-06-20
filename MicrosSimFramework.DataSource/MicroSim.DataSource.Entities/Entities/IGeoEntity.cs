using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.Entities
{
    /// <summary>
    /// Interface controlling the Geographic property of raw datasets
    /// </summary>
    public interface IGeoEntity
    {
        /// <summary>
        /// Gets or sets the geographic location.
        /// </summary>
        /// <value>
        /// The geographic location.
        /// </value>
        string Geo { get; set; }
    }
}
