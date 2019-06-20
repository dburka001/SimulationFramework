using MicroSim.DataSource.Core;
using MicroSim.DataSource.Entities;
using MicroSim.DataSource.Rtools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.Mortality
{
    /// <summary>
    /// DspMortalityEduForecast class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSourcePart" />
    public class DspMortalityEduForecast : MsfDataSourcePart<MortalityEduBaseEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DspPopulationEduBase"/> class.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        public DspMortalityEduForecast(params MsfDataSourcePart[] inputs)
            : base(inputs) { }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.DspMortalityEduForecast;

        /// <summary>
        /// Generates the data.
        /// </summary>
        protected override void GenerateData()
        {
            var mortalityForecast = new List<MortalityEduBaseEntity>();
            var mortality = GetInputDataOfType<MortalityEduBaseEntity>();

            foreach (var m in mortality)
            {
                mortalityForecast.Add(new MortalityEduBaseEntity()
                {
                    Age = m.Age,
                    DeathCount = m.DeathCount,
                    Education = m.Education,
                    Gender = m.Gender,
                    Population = m.Population,
                    Value = m.Value,
                    Year = m.Year
                });
            }

            ForecastTotals(mortalityForecast);
            ForecastEduGroups(mortalityForecast);

            InterpolateForecasts(mortalityForecast);

            Data = mortalityForecast;
        }

        /// <summary>
        /// Interpolates the forecasts.
        /// </summary>
        /// <param name="mortalityForecast">The mortality forecast.</param>
        private void InterpolateForecasts(List<MortalityEduBaseEntity> mortalityForecast)
        {
            var interpolateFromAge = 100;
            var groups = mortalityForecast
                .Select(m => new { m.Gender, m.Education })
                .Distinct()
                .ToList();

            foreach (var g in groups)
            {
                var forecastYear = mortalityForecast
                    .Where(m => m.Gender == g.Gender && m.Education == g.Education)
                    .OrderBy(m => m.Age)
                    .ToList();

                if (interpolateFromAge < Settings.AgeLimit)
                {
                    var values = forecastYear
                            .Select(f => f.Age >= interpolateFromAge && f.Age != Settings.AgeLimit
                                    ? Double.NaN
                                    : Convert.ToDouble(f.Value))
                            .ToArray();

                    double[] newValues =
                        Rscripts.InterpolateMortality(values);

                    for (int i = 0; i < forecastYear.Count; i++)
                    {
                        forecastYear[i].Value = Convert.ToDecimal(newValues[i]);
                    }
                }
            }
        }

        /// <summary>
        /// Forecasts the education groups.
        /// </summary>
        /// <param name="mortalityForecast">The mortality forecast.</param>
        private void ForecastEduGroups(List<MortalityEduBaseEntity> mortalityForecast)
        {
            var multipliers = GetInputDataOfType<MortalityEduMultiplierEntity>();

            var avgMultipliers = multipliers
                .GroupBy(m => new { m.Gender, m.Education, m.Age })
                .Select(g => new
                {
                    g.Key.Gender,
                    g.Key.Education,
                    g.Key.Age,
                    Value = g.Average(m => m.Value)
                });

            var mortalityEduMaxYear = mortalityForecast
                .Where(m => m.Education != Education.Total)
                .Max(m => m.Year);

            var mortalityEduForecast = mortalityForecast
                .Where(m => m.Education == Education.Total && m.Year > mortalityEduMaxYear)
                .Join(avgMultipliers,
                m => new { m.Gender, m.Age },
                a => new { a.Gender, a.Age },
                (m, a) => new MortalityEduBaseEntity
                {
                    Age = m.Age,
                    Education = a.Education,
                    Gender = m.Gender,
                    Year = m.Year,
                    Value = m.Value * a.Value
                })
                .ToList();

            mortalityForecast.AddRange(mortalityEduForecast);
        }

        /// <summary>
        /// Forecasts the totals.
        /// </summary>
        /// <param name="mortalityForecast">The mortality forecast.</param>
        private void ForecastTotals(List<MortalityEduBaseEntity> mortalityForecast)
        {
            var groups = mortalityForecast
                .Select(m => new { m.Gender })
                .Distinct()
                .ToList();

            foreach (var g in groups)
            {
                var matrices = DemographyExtensions.GetDemographyMatrices(
                    mortalityForecast
                    .Where(m => 
                    m.Year >= Settings.MortalityForecastFromYear &&
                    m.Gender == g.Gender && 
                    m.Education == Education.Total));

                var forecast = Rscripts.ForecastDemography(
                    "mortality",
                    matrices.Item1,
                    matrices.Item2,
                    matrices.Item3,
                    matrices.Item4,
                    Settings.ForecastYears);

                for (int year = 1; year <= Settings.ForecastYears; year++)
                {
                    var forecastYear = new List<MortalityEduBaseEntity>();                    

                    foreach (var age in matrices.Item3)
                    {

                        forecastYear.Add(new MortalityEduBaseEntity()
                        {
                            Age = age,
                            Gender = g.Gender,
                            Education = Education.Total,
                            DeathCount = null,
                            Population = null,
                            Year = (Settings.StartYear + year),
                            Value = Convert.ToDecimal(Math.Min(1, forecast.Matrix[age, year - 1])),
                            KtLower = Convert.ToDecimal(forecast.KtLower[year - 1]),
                            Kt = Convert.ToDecimal(forecast.Kt[year - 1]),
                            KtUpper = Convert.ToDecimal(forecast.KtUpper[year - 1])
                        });
                    }

                    forecastYear.OrderBy(f => f.Age);
                    

                    mortalityForecast.AddRange(forecastYear);
                }
            }
        }
    }
}
