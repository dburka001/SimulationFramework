using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.Entities
{
    /// <summary>
    /// DataFileType enum
    /// </summary>
    public enum DataFileType { CSV, TSV }

    /// <summary>
    /// DataFileTypeExtensions
    /// </summary>
    public static class DataFileTypeExtensions
    {
        /// <summary>
        /// Parses the specified DFT.
        /// </summary>
        /// <param name="dft">The DFT.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public static DataFileType Parse(string dft)
        {
            switch (dft)
            {
                case ".csv": return DataFileType.CSV;
                case ".tsv": return DataFileType.TSV;
                default: throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets the Extension.
        /// </summary>
        /// <param name="dft">The DFT.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public static string GetExtension(this DataFileType dft)
        {
            switch (dft)
            {
                case DataFileType.CSV: return ".csv";
                case DataFileType.TSV: return ".tsv";
                default: throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets the base delimiter.
        /// </summary>
        /// <param name="dft">The DFT.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public static string GetBaseDelimiter(this DataFileType dft)
        {
            switch (dft)
            {
                case DataFileType.CSV: return ";";
                case DataFileType.TSV: return "\t";
                default: throw new NotImplementedException();
            }
        }
    }
}
