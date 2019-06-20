using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.Entities
{
    /// <summary>
    /// PensionType enum
    /// </summary>
    public enum PensionType : byte { Total, Base, Variable, Hybrid }

    /// <summary>
    /// PensionTypeExtensions
    /// </summary>
    public static class PensionTypeExtensions
    {
        /// <summary>
        /// Parses the specified gender.
        /// </summary>
        /// <param name="pensionType">The gender.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public static PensionType Parse(string pensionType)
        {            
            switch (pensionType)
            {
                case "0": return PensionType.Total;
                case "1": return PensionType.Base;
                case "2": return PensionType.Variable;
                case "3": return PensionType.Hybrid;
                default: return (PensionType)Enum.Parse(typeof(PensionType), pensionType);
            }
        }
    }
}
