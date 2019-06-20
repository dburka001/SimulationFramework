using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.Entities
{
    /// <summary>
    /// Interface for handling Age group Start and End years
    /// </summary>
    public interface IAgeIntervalEntity
    {
        /// <summary>
        /// Gets or sets the age.
        /// </summary>
        /// <value>
        /// The age.
        /// </value>
        string AgeInterval { get; set; }
    }

    /// <summary>
    /// AgeIntervalExtensions
    /// </summary>
    public static class AgeIntervalExtensions
    {
        /// <summary>
        /// Gets the age interval.
        /// </summary>
        /// <param name="ageInterval">The age interval.</param>
        /// <returns></returns>
        public static Tuple<int, int> GetAgeInterval(string ageInterval)
        {
            string start = "";
            string end = "";

            if (ageInterval.Contains(" - "))
            {
                start = ageInterval.Substring(0, ageInterval.IndexOf(" - "));
                end = ageInterval.Substring(ageInterval.IndexOf(" - ") + 3);
                if (end.Contains("x")) end = Settings.AgeLimit.ToString();
            }
            else if(ageInterval.Contains("-"))
            {
                start = ageInterval.Substring(1, ageInterval.IndexOf("-") - 1);
                end = ageInterval.Substring(ageInterval.IndexOf("-") + 1);
            }
            else
            {
                if (ageInterval == "51+")
                {
                    start = "51";
                    end = Settings.AgeLimit.ToString();
                }
                else
                {
                    start = ageInterval;
                    end = start;
                }
            }
            return Tuple.Create(int.Parse(start), int.Parse(end));
        }
    }
}
