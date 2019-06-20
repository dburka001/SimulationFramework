using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.Entities
{
    /// <summary>
    /// LogLevel enum
    /// </summary>
    public enum LogLevel : byte { Default, Important, Warning, Error }

    /// <summary>
    /// LogLevelExtensions class
    /// </summary>
    public static class LogLevelExtensions
    {
        /// <summary>
        /// Gets the color.
        /// </summary>
        /// <param name="logLevel">The logLevel.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public static Color GetColor(this LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Default: return Color.Black;
                case LogLevel.Important: return Color.Green;
                case LogLevel.Warning: return Color.Yellow;
                case LogLevel.Error: return Color.Red;
                default: throw new NotImplementedException();
            }
        }
    }
}
