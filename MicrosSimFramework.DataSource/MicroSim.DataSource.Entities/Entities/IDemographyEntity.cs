using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.Entities
{
    /// <summary>
    /// Interface for handling demographic rates
    /// </summary>
    public interface IDemographyEntity : IPopulationEntity
    {
        decimal? Population { get; set; }
        decimal? DeathCount { get; set; }
        decimal? BirthCount { get; set; }
        decimal? KtLower { get; set; }
        decimal? Kt { get; set; }
        decimal? KtUpper { get; set; }
    }

    /// <summary>
    /// DemographyExtensions class
    /// </summary>
    public static class DemographyExtensions
    {
        /// <summary>
        /// Recalculates values according to the Böckh-formula.
        /// </summary>
        /// <param name="data">The data.</param>
        public static void BockhFormula(IEnumerable<IDemographyEntity> data)
        {
            var orderedData = data
                    .OrderBy(d => d.Age);

            var ages = orderedData
                .Select(d => d.Age)
                .ToArray();

            if (ages.Length != Settings.AgeLimit + 1)
                throw new Exception(Resources.ExMissingAges);

            var population = orderedData
                    .Select(d => d.Population)
                    .ToArray();                             

            var death = orderedData
                    .Select(d => d.DeathCount)
                    .ToArray();

            var bockhValues = new decimal[ages.Length];

            int i = 0;
            foreach (var d in orderedData)
            {
                var Px = population[i];

                if (i == ages.Length - 1 || Px == 0)
                {
                    d.Value = 1;
                    continue;
                }
                               
                var Dx1 = death[i] / 2;
                var Dx2 = death[i + 1] / 2;                                
                decimal? p = 0;

                if (i == 0)
                    p = (d.BirthCount - Dx1) / d.BirthCount;
                else
                {
                    var Dx01 = death[i - 1] / 2;
                    var Px0 = population[i - 1];

                    if (Px0 - Dx01 == 0)
                    {
                        d.Value = 1;
                        continue;
                    }
                    p = (Px0 - Dx01 - Dx1) / (Px0 - Dx01); 
                }

                p *= (Px - Dx2) / Px;

                d.Value = Math.Max(0, Math.Min((decimal)(1 - p), 1));

                i++;
            }
        }        

        /// <summary>
        /// Gets the demography matrices needed for forecasting.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static Tuple<double[,], double[,], int[], int[]> GetDemographyMatrices(IEnumerable<IDemographyEntity> data)
        {            
            int[] ages = data
                .OrderBy(d => d.Age)
                .Select(d => d.Age)
                .Distinct()
                .ToArray();
            int[] years = data
                .OrderBy(d => d.Year)
                .Where(d => d.Year <= Settings.StartYear)
                .Select(d => d.Year)
                .Distinct()
                .ToArray();

            double[,] populationMatrix = new double[ages.Length, years.Length];
            double[,] demographyMatrix = new double[ages.Length, years.Length];

            foreach (var d in data)
            {
                int ageIndex = Array.IndexOf(ages, d.Age);
                int yearIndex = Array.IndexOf(years, d.Year);
                populationMatrix[ageIndex, yearIndex] = d.Population == null ? 0 : Convert.ToDouble(d.Population);
                demographyMatrix[ageIndex, yearIndex] = d.Value == null ? 0 : Convert.ToDouble(d.Value);
            }

            return new Tuple<double[,], double[,], int[], int[]>(demographyMatrix, populationMatrix, ages, years);
        }
    }
}
