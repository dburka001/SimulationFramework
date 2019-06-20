using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.Entities
{
    /// <summary>
    /// Education enum
    /// </summary>
    public enum Education : byte { Total, ED0_2, ED3_4, ED5_8, UNK, NAP }

    /// <summary>
    /// GenderExtensions
    /// </summary>
    public static class EducationExtensions
    {
        /// <summary>
        /// Parses the specified education.
        /// </summary>
        /// <param name="education">The education.</param>
        /// <returns></returns>
        public static Education Parse(string education)
        {
            switch (education)
            {
                case "TOTAL": return Education.Total;
                case "ED0-2": return Education.ED0_2;
                case "ED3_4": return Education.ED3_4;
                case "ED5-8": return Education.ED5_8;
                case "0": return Education.ED0_2;
                case "1": return Education.ED3_4;
                case "2": return Education.ED5_8;
                case "NRP": return Education.UNK;
                case "NAP": return Education.NAP;
                case "UNK": return Education.UNK;
                case "Ismeretlen": return Education.UNK;
                case "Általános iskola 0-7. osztálya": return Education.ED0_2;
                case "Befejezett általános iskola (8. osztály)": return Education.ED0_2;
                case "Befejezett szakmunkásképző iskola, szakiskola": return Education.ED3_4;
                case "Befejezett középiskola": return Education.ED3_4;
                case "Befejezett felsőfokú iskola": return Education.ED5_8;
                default: return (Education)Enum.Parse(typeof(Education), education);
            }
        }

        /// <summary>
        /// Gets the label.
        /// </summary>
        /// <param name="education">The education.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static string GetLabel(this Education education)
        {
            switch (education)
            {
                case Education.Total: return Resources.Total;
                case Education.ED0_2: return Resources.EducationED02;
                case Education.ED3_4: return Resources.EducationED34;
                case Education.ED5_8: return Resources.EducationED58;
                case Education.UNK: return Resources.Unknown;
                case Education.NAP: return Resources.NAP;
                default: throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets the minimum age.
        /// </summary>
        /// <param name="education">The education.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static int? GetMinimumAge(this Education education)
        {
            switch (education)
            {
                case Education.Total: return 0;
                case Education.ED0_2: return 0;
                case Education.ED3_4: return 19;
                case Education.ED5_8: return 21;
                case Education.UNK: return 0;
                default: throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Distributes the unknowns in an education dataset.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static IEnumerable<T> DistributeUnknowns<T>(
            IEnumerable<IPopulationEduEntity> data)
        {
            var unknowns = data.Where(d => d.Education == Education.UNK);

            foreach (var u in unknowns)
            {
                if (u.Value == null || u.Value == 0) continue;

                var currentData = data
                    .Where(d =>
                    d.Age == u.Age &&
                    d.Gender == u.Gender &&
                    d.Year == u.Year &&                    
                    d.Education != Education.UNK);

                decimal? sum = currentData.Sum(d => d.Value);
                if (sum == null || sum == 0) continue;

                foreach (var c in currentData)                
                    c.Value += (decimal)(u.Value * c.Value / sum);                
            }
            
            return data.Except(unknowns).Cast<T>();
        }
    }
}
