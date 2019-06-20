using MicroSim.DataSource.Core;
using MicroSim.DataSource.Entities;
using MicroSim.DataSource.Rtools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.Fertility
{
    /// <summary>
    /// DspFertilityForecast class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSourcePart" />
    public class DspFertilityForecast : MsfDataSourcePart<FertilityCompleteBaseEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DspFertilityForecast"/> class.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        public DspFertilityForecast(params MsfDataSourcePart[] inputs)
            : base(inputs) { }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.DspFertilityForecast;

        /// <summary>
        /// Generates the data.
        /// </summary>
        protected override void GenerateData()
        {
            var FertilityForecast = new List<FertilityCompleteBaseEntity>();
            var Fertility = GetInputDataOfType<FertilityCompleteBaseEntity>();

            foreach (var m in Fertility.Where(f => f.Year >= Settings.FertilityForecastFromYear))
            {
                FertilityForecast.Add(new FertilityCompleteBaseEntity()
                {
                    Age = m.Age,
                    DeathCount = m.DeathCount,
                    Education = m.Education,
                    BirthOrder = m.BirthOrder,
                    Gender = m.Gender,
                    Population = m.Population,
                    Value = m.Value,
                    Year = m.Year
                });
            }

            ForecastTotals(FertilityForecast);
            ForecastGroups(FertilityForecast);

            Data = FertilityForecast;
        }

        /// <summary>
        /// Forecasts the education groups.
        /// </summary>
        /// <param name="FertilityForecast">The Fertility forecast.</param>
        private void ForecastGroups(List<FertilityCompleteBaseEntity> FertilityForecast)
        {
            var multipliers = GetInputDataOfType<FertilityEduMultiplierEntity>();

            var avgMultipliers = multipliers
                .GroupBy(m => new { m.Gender, m.Education, m.BirthOrder, m.Age })
                .Select(g => new
                {
                    g.Key.Gender,
                    g.Key.Education,
                    g.Key.BirthOrder,
                    g.Key.Age,
                    Value = g.Average(m => m.Value)
                });

            var FertilityEduMaxYear = FertilityForecast
                .Where(m => !(m.Education == Education.Total && m.BirthOrder == BirthOrder.Total))
                .Max(m => m.Year);

            var FertilityEduForecast = FertilityForecast
                .Where(m => 
                m.Education == Education.Total && 
                m.BirthOrder == BirthOrder.Total && 
                m.Year > FertilityEduMaxYear)
                .Join(avgMultipliers,
                m => new { m.Gender, m.Age },
                a => new { a.Gender, a.Age },
                (m, a) => new FertilityCompleteBaseEntity
                {
                    Age = m.Age,
                    Education = a.Education,
                    BirthOrder = a.BirthOrder,
                    Gender = m.Gender,
                    Year = m.Year,
                    Value = m.Value * a.Value
                })
                .ToList();

            FertilityForecast.AddRange(FertilityEduForecast);
        }

        /// <summary>
        /// Forecasts the totals.
        /// </summary>
        /// <param name="FertilityForecast">The Fertility forecast.</param>
        private void ForecastTotals(List<FertilityCompleteBaseEntity> FertilityForecast)
        {
            var groups = FertilityForecast
                .Select(m => new { m.Gender })
                .Distinct()
                .ToList();

            foreach (var g in groups)
            {
                var matrices = DemographyExtensions.GetDemographyMatrices(
                    FertilityForecast
                    .Where(m => 
                    m.Gender == g.Gender && 
                    m.Education == Education.Total &&
                    m.BirthOrder == BirthOrder.Total));

                var forecast = Rscripts.ForecastDemography(
                    "fertility",
                    matrices.Item1,
                    matrices.Item2,
                    matrices.Item3,
                    matrices.Item4,
                    Settings.ForecastYears);

                var minAge = matrices.Item3.Min();

                foreach (var age in matrices.Item3)
                {
                    for (int year = 1; year <= Settings.ForecastYears; year++)
                    {
                        FertilityForecast.Add(new FertilityCompleteBaseEntity()
                        {
                            Age = age,
                            Gender = g.Gender,
                            Education = Education.Total,
                            BirthOrder = BirthOrder.Total,
                            DeathCount = null,
                            Population = null,
                            Year = (Settings.StartYear + year),
                            Value = Convert.ToDecimal(Math.Min(1, forecast.Matrix[age - minAge, year - 1])),
                            KtLower = Convert.ToDecimal(forecast.KtLower[year - 1]),
                            Kt = Convert.ToDecimal(forecast.Kt[year - 1]),
                            KtUpper = Convert.ToDecimal(forecast.KtUpper[year - 1])
                        });
                    }
                }
            }
        }
    }
}
