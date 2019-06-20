using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.Entities
{
    /// <summary>
    /// DetEducationType enum
    /// </summary>
    public enum DetEducationType : byte { Total, Normal, Pessimistic, Optimistic }

    /// <summary>
    /// DetEducationTypeExtensions
    /// </summary>
    public static class DetEducationTypeExtensions
    {
        public static DetEducationType Parse(string detEducationType)
        {
            switch (detEducationType)
            {
                case "0": return DetEducationType.Total;
                case "1": return DetEducationType.Normal;
                case "2": return DetEducationType.Pessimistic;
                case "3": return DetEducationType.Optimistic;
                default: return (DetEducationType)Enum.Parse(typeof(DetEducationType), detEducationType);
            }
        }
    }
}
