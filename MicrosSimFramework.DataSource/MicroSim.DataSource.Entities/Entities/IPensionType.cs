using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.Entities
{
    /// <summary>
    /// Interface for handling the PensionTypes
    /// </summary>
    public interface IPensionType
    {
        /// <summary>
        /// Gets or sets the type of the pension.
        /// </summary>
        /// <value>
        /// The type of the pension.
        /// </value>
        [MsfDisplayName(nameof(PensionType))]
        PensionType PensionType { get; set; }
    }
}
