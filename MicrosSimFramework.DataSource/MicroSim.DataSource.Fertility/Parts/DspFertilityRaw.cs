using MicroSim.DataSource.Core;
using MicroSim.DataSource.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.Fertility
{
    /// <summary>
    /// DspFertilityRaw class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSourcePart" />
    public class DspFertilityRaw : MsfDataSourcePart<FertilityCompleteBaseEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DspFertilityRaw"/> class.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        public DspFertilityRaw(params MsfDataSourcePart[] inputs)
            : base(inputs) { }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.DspFertilityRaw;

        /// <summary>
        /// Generates the data.
        /// </summary>
        protected override void GenerateData()
        {
            var mothers = GetInputDataOfType<PopulationBaseEntity>();
            var children = GetInputDataOfType<BirthBaseEntity>();

            var childrenBase = children
                .GroupBy(c => new { c.Age, c.Gender, c.Year })
                .Select(g => new
                {
                    g.Key.Age,
                    g.Key.Gender,
                    g.Key.Year,
                    Value = g.Sum(c => c.Value)
                });

            var birthProbabilities = mothers
                .Where(m => m.Gender == Gender.Female)
                .Join(childrenBase,
                m => new { m.Age, m.Gender, m.Year },
                c => new { c.Age, c.Gender, c.Year },
                (m, c) => new FertilityCompleteBaseEntity()
                {
                    Age = m.Age,
                    Gender = m.Gender,
                    Year = m.Year,
                    BirthOrder = BirthOrder.Total,
                    Education = Education.Total,
                    Population = m.Value,
                    BirthCount = c.Value,
                    Value = MsfExtensions.GetProbabilityValue(m.Value, c.Value)
                })
                .ToList();

            var mothersComplete = GetInputDataOfType<BirthMotherEntity>();
            var childrenComplete = GetInputDataOfType<BirthCompleteEntity>();

            var birthCompleteProbabilities = mothersComplete
                .Join(childrenComplete,
                m => new { m.Age, m.Gender, m.Year, m.Education, m.BirthOrder },
                c => new { c.Age, c.Gender, c.Year, c.Education, c.BirthOrder},
                (m, c) => new FertilityCompleteBaseEntity()
                {
                    Age = m.Age,
                    Gender = m.Gender,
                    Year = m.Year,
                    BirthOrder = m.BirthOrder,
                    Education = m.Education,
                    Population = m.Value,
                    BirthCount = c.Value,
                    Value = Math.Min(MsfExtensions.GetProbabilityValue(m.Value, c.Value) ?? 0, (decimal)0.2)
                })
                .ToList();

            birthProbabilities.AddRange(birthCompleteProbabilities);

            Data = birthProbabilities;
        }
    }
}
