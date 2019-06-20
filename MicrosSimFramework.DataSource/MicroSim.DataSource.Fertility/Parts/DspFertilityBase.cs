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
    /// DspFertilityBase class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSourcePart" />
    public class DspFertilityBase : MsfDataSourcePart<FertilityCompleteBaseEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DspFertilityBase"/> class.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        public DspFertilityBase(params MsfDataSourcePart[] inputs)
            : base(inputs) { }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.DspFertilityBase;

        /// <summary>
        /// Generates the data.
        /// </summary>
        protected override void GenerateData()
        {
            var Fertility = new List<FertilityCompleteBaseEntity>();
            var FertilityRaw = GetInputDataOfType<FertilityCompleteBaseEntity>();

            foreach (var m in FertilityRaw)
            {
                Fertility.Add(new FertilityCompleteBaseEntity()
                {
                    Age = m.Age,
                    DeathCount = m.DeathCount,
                    Education = m.Education,
                    Gender = m.Gender,
                    Population = m.Population,
                    BirthOrder = m.BirthOrder,
                    BirthCount = m.BirthCount,
                    Value = m.Value,
                    Year = m.Year
                });
            }

            var groups = Fertility
                .Select(m => new { m.Gender, m.Education, m.BirthOrder, m.Year })
                .Distinct();

            foreach (var g in groups)
            {                
                var currentData = Fertility
                    .Where(m =>
                    m.Gender == g.Gender &&
                    m.Education == g.Education &&
                    m.BirthOrder == g.BirthOrder &&
                    m.Year == g.Year);
                
                if(Settings.SmoothProbabilities)
                    MsfExtensions.SmoothValues(currentData
                        .Where(d => d.Age != Settings.AgeLimit));
            }

            Data = Fertility;
        }
    }
}
