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
    /// DspMortalityEduBase class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSourcePart" />
    public class DspMortalityEduBase : MsfDataSourcePart<MortalityEduBaseEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DspPopulationEduBase"/> class.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        public DspMortalityEduBase(params MsfDataSourcePart[] inputs)
            : base(inputs) { }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.DspMortalityEduBase;

        /// <summary>
        /// Generates the data.
        /// </summary>
        protected override void GenerateData()
        {
            var mortality = new List<MortalityEduBaseEntity>();
            var mortalityRaw = GetInputDataOfType<MortalityEduBaseEntity>();

            foreach (var m in mortalityRaw)
            {
                mortality.Add(new MortalityEduBaseEntity()
                {
                    Age = m.Age,
                    DeathCount = m.DeathCount,
                    Education = m.Education,
                    Gender = m.Gender,
                    Population = m.Population,
                    BirthCount = m.BirthCount,
                    Value = m.Value,
                    Year = m.Year
                });
            }

            var groups = mortality
                .Select(m => new { m.Gender, m.Education, m.Year })
                .Distinct();

            foreach (var g in groups)
            {                
                var currentData = mortality
                    .Where(m =>
                    m.Gender == g.Gender &&
                    m.Education == g.Education &&
                    m.Year == g.Year);

                if (Settings.SmoothProbabilities)
                {
                    DemographyExtensions.BockhFormula(currentData);
                    MsfExtensions.SmoothValues(currentData
                        .Where(d => d.Age != Settings.AgeLimit));
                }
            }

            Data = mortality;
        }
    }
}
