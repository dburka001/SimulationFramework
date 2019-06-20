using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.Entities
{
    /// <summary>
    /// Gender enum
    /// </summary>
    public enum Gender : byte { Total, Male, Female }

    /// <summary>
    /// GenderExtensions
    /// </summary>
    public static class GenderExtensions
    {
        /// <summary>
        /// Parses the specified gender.
        /// </summary>
        /// <param name="gender">The gender.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public static Gender Parse(string gender)
        {            
            switch (gender)
            {
                case "T": return Gender.Total;
                case "M": return Gender.Male;
                case "F": return Gender.Female;
                case "1": return Gender.Male;
                case "2": return Gender.Female;
                default: return (Gender)Enum.Parse(typeof(Gender), gender);
            }
        }
    }
}
