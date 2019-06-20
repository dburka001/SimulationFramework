using MicroSim.DataSource.Core;
using MicroSim.DataSource.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.StartingPopulation
{
    /// <summary>
    /// DspStartWithMinoritiesAndMigrants class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSourcePart" />
    public class DspStartWithMinoritiesAndMigrants : MsfDataSourcePart<StartingEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DspStartWithMinoritiesAndMigrants"/> class.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        public DspStartWithMinoritiesAndMigrants(params MsfDataSourcePart[] inputs) 
            : base(inputs) { }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.DspStartWithMinoritiesAndMigrants;

        /// <summary>
        /// Generates the data.
        /// </summary>
        protected override void GenerateData()
        {
            var output = new List<StartingEntity>();
            var data = GetInputDataOfType<StartingEntity>();
            var imigrantSettings = GetInputDataOfType<ImmigrantSettingsEntity>();
            var baseData = data.Where(d => d.SocialGroup == SocialGroup.Majority);

            foreach (var m in imigrantSettings)
            {
                if (m.Year < 0 || m.Year > Settings.ForecastYears) continue;

                var yearDistribution = GetNormalDistribution(m.AgeMean, m.AgeVariance);

                for (int a = 0; a <= Settings.AgeLimit; a++)
                {
                    if (yearDistribution[a] == 0) continue;

                    var current = baseData.Where(d => d.Age == a);
                    decimal? currentTotal = current.Sum(d => d.Value);
                    if (currentTotal == null || currentTotal == 0) continue;

                    foreach (var c in current)
                    {
                        var currentValue = yearDistribution[a] * m.Value * c.Value / currentTotal;
                        if (Math.Round((decimal)currentValue, 0) == 0) continue;

                        output.Add(new StartingEntity()
                        {
                            SocialGroup = SocialGroup.Immigrant,
                            ActiveYear = Settings.StartYear + m.Year,
                            Age = c.Age,
                            Gender = c.Gender,
                            Education = c.Education,
                            BirthOrder = c.BirthOrder,
                            Year = c.Year,                            
                            Value = currentValue
                        });
                    }
                }
            }

            output.AddRange(data);

            Data = output;
        }


        /// <summary>
        /// Gets the normal distribution.
        /// </summary>
        /// <param name="m">The m.</param>
        /// <param name="variance">The variance.</param>
        /// <returns></returns>
        private decimal[] GetNormalDistribution(decimal m, decimal variance)
        {
            decimal[] output = new decimal[Settings.AgeLimit + 1];
            decimal sigma = (decimal)Math.Sqrt((double)variance);
            decimal szorzó = 1 / (sigma * (decimal)Math.Sqrt(2.0 * Math.PI));

            for (int x = 0; x < output.Length; x++)
            {
                output[x] = szorzó * (decimal)Math.Exp(- Math.Pow(x - (double)m, 2) / (2.0 * Math.Pow((double)sigma, 2)));
            }

            return output;
        }
    }
}
