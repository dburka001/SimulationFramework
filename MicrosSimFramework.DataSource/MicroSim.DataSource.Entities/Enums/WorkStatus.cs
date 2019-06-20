using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.Entities
{
    /// <summary>
    /// WorkStatus enum
    /// </summary>
    public enum WorkStatus : byte { A, B1, B2, B3, B4, G }

    /// <summary>
    /// WorkStatusExtensions
    /// </summary>
    public static class WorkStatusExtensions
    {
        /// <summary>
        /// Parses the specified work status.
        /// </summary>
        /// <param name="workStatus">The work status.</param>
        /// <returns></returns>
        public static WorkStatus Parse(string workStatus)
        {            
            switch (workStatus)
            {
                case "A": return WorkStatus.A;
                case "B1": return WorkStatus.B1;
                case "B2": return WorkStatus.B2;
                case "B3": return WorkStatus.B3;
                case "B4": return WorkStatus.B4;
                case "G": return WorkStatus.G;                
                default: return (WorkStatus)Enum.Parse(typeof(WorkStatus), workStatus);
            }
        }
    }
}
