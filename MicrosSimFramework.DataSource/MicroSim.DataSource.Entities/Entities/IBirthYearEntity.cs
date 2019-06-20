using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.Entities
{
    /// <summary>
    /// Interface for handling the BirthYears
    /// </summary>
    public interface IBirthYearEntity
    {
        /// <summary>
        /// Gets or sets the birth year.
        /// </summary>
        /// <value>
        /// The birth year.
        /// </value>
        [MsfDisplayName(nameof(BirthYear))]
        int BirthYear { get; set; }
    }
}
