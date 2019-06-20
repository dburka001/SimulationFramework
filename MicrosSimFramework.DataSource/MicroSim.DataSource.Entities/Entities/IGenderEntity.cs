using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.Entities
{
    /// <summary>
    /// Interface for handling the Genders
    /// </summary>
    public interface IGenderEntity
    {
        /// <summary>
        /// Gets or sets the gender.
        /// </summary>
        /// <value>
        /// The gender.
        /// </value>
        [MsfDisplayName(nameof(Gender))]
        Gender Gender { get; set; }
    }
}
