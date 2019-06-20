using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.Entities
{
    /// <summary>
    /// BirthOrder enum
    /// </summary>
    public enum BirthOrder : byte { Total, B0, B1, B2, B3, B4, B5more }

    /// <summary>
    /// BirthOrderExtensions
    /// </summary>
    public static class BirthOrderExtensions
    {
        /// <summary>
        /// Parses the specified birth order.
        /// </summary>
        /// <param name="birthOrder">The birth order.</param>
        /// <returns></returns>
        public static BirthOrder Parse(string birthOrder)
        {            
            switch (birthOrder)
            {
                case "0": return BirthOrder.B0;
                case "1": return BirthOrder.B1;
                case "2": return BirthOrder.B2;
                case "3": return BirthOrder.B3;
                case "4": return BirthOrder.B4;
                case "5": return BirthOrder.B5more;
                case "5+": return BirthOrder.B5more;
                default: return (BirthOrder)Enum.Parse(typeof(BirthOrder), birthOrder);
            }
        }
    }
}
