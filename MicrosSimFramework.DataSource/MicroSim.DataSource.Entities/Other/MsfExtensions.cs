using MicroSim.DataSource.Rtools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MicroSim.DataSource.Entities
{
    /// <summary>
    /// MsfExtensions
    /// </summary>
    public static class MsfExtensions
    {
        /// <summary>
        /// Gets the probability value.
        /// </summary>
        /// <param name="populationValue">The population value.</param>
        /// <param name="otherValue">The other value.</param>
        /// <returns></returns>
        public static decimal? GetProbabilityValue(decimal? populationValue, decimal? otherValue)
        {
            if (otherValue == null || populationValue == null)
                return null;
            if (populationValue == 0)
                return 1;

            var outputValue = (decimal?)Math.Min((decimal)(otherValue / populationValue), 1);

            return outputValue;
        }

        /// <summary>
        /// Smoothes the values.
        /// </summary>
        /// <param name="data">The data.</param>
        public static void SmoothValues(IEnumerable<IValueEntity> data)
        {
            var values = data
                   .Select(d => d.Value)
                   .ToArray();

            var newValues = Rscripts.SmoothValues(
                Array.ConvertAll(values, v => (double)v));

            var i = 0;
            foreach (var d in data)
            {
                d.Value = Convert.ToDecimal(newValues[i]);
                i++;
            }
        }

        /// <summary>
        /// Distributes the det education values.
        /// </summary>
        /// <param name="data">The data.</param>
        public static void DistributeDetEducationValues(IEnumerable<IPopulationEduEntity> data)
        {
            var groups = data
                .Where(d => d.Age <= Settings.DetEducationAgeStart)
                .GroupBy(d => new { d.Gender, d.Year })
                .Select(g => new
                {
                    g.Key.Gender,
                    g.Key.Year
                });
            var ages = data
                .Where(d => d.Age < Settings.DetEducationAgeStart)
                .Select(d => d.Age)
                .Distinct();

            foreach (var g in groups)
            {
                var groupValues = data
                    .Where(d =>
                    d.Gender == g.Gender &&
                    d.Year == g.Year);

                var topValues = groupValues
                    .Where(v => v.Age == Settings.DetEducationAgeStart);

                decimal? topSum = topValues
                    .Where(v => v.Value != null)
                    .Sum(v => v.Value);

                foreach (var a in ages)
                {
                    var currentValues = groupValues
                        .Where(v => v.Age == a);

                    decimal? currentSum = currentValues
                        .Where(v => v.Value != null)
                        .Sum(v => v.Value);

                    foreach (var c in currentValues)
                    {
                        var currentTop = topValues
                            .Where(t =>
                            t.Education == c.Education &&
                            t.Gender == c.Gender &&
                            t.Year == c.Year)
                            .FirstOrDefault();

                        if (currentTop == null || topSum == null || topSum == 0)
                            c.Value = null;
                        else
                            c.Value = currentSum * currentTop.Value / topSum;
                    }
                }
            }           
        }
    }
}
