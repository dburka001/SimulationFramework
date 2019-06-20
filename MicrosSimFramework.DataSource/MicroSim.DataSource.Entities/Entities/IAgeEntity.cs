using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.Entities
{
    /// <summary>
    /// Interface for handling Age of entities
    /// </summary>
    public interface IAgeEntity
    {
        /// <summary>
        /// Gets or sets the age.
        /// </summary>
        /// <value>
        /// The age.
        /// </value>
        [MsfDisplayName(nameof(Age))]
        int Age { get; set; }
    }

    /// <summary>
    /// AgeExtensions
    /// </summary>
    public static class AgeExtensions
    {
        /// <summary>
        /// Gets the correct age.
        /// </summary>
        /// <param name="age">The age.</param>
        /// <returns></returns>
        public static int? GetCorrectAge(string age)
        {
            switch (age)
            {
                case "Y_LT1": return 0;
                case "Y_OPEN": return 100;
                case "Y_GE50": return 50;
                case "Y10-14": return 14;
                case "Y_NAP": return -1;
                case "TOTAL": return null;
                case "UNK": return null;
                default: return int.Parse(age.Substring(1));
            }
        }
    }
}
