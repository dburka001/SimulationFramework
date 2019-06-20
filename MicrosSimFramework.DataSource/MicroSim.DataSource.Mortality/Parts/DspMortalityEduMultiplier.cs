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
    /// DspMortalityEduMultipliers class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSourcePart" />
    public class DspMortalityEduMultiplier : MsfDataSourcePart<MortalityEduMultiplierEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DspMortalityEduMultiplier"/> class.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        public DspMortalityEduMultiplier(params MsfDataSourcePart[] inputs)
            : base(inputs) { }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.DspMortalityEduMultipliers;

        /// <summary>
        /// Generates the data.
        /// </summary>
        protected override void GenerateData()
        {
            var mortality = GetInputDataOfType<MortalityEduBaseEntity>();

            var eduData = mortality.Where(m => m.Education != Education.Total);
            var totalData = mortality.Where(m => m.Education == Education.Total);

            var mortalityMultipliers = eduData
                .Join(totalData,
                e => new { e.Year, e.Age, e.Gender },
                t => new { t.Year, t.Age, t.Gender },
                (e, t) => new MortalityEduMultiplierEntity
                {
                    Year = e.Year,
                    Age = e.Age,
                    Gender = e.Gender,
                    Education = e.Education,
                    Value = t.Value == 0 ? 1 : e.Value / t.Value
                });

            Data = mortalityMultipliers;
        }     
    }
}
