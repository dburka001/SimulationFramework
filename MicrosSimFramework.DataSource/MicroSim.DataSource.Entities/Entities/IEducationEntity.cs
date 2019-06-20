using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.Entities
{
    /// <summary>
    /// Interface for handling the Education attainments
    /// </summary>
    public interface IEducationEntity
    {
        /// <summary>
        /// Gets or sets the education.
        /// </summary>
        /// <value>
        /// The education.
        /// </value>
        [MsfDisplayName(nameof(Education))]
        Education Education { get; set; }
    }
}
