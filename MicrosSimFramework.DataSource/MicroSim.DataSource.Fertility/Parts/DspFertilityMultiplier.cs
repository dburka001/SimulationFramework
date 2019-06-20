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
    /// DspFertilityMultiplier class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSourcePart" />
    public class DspFertilityMultiplier : MsfDataSourcePart<FertilityEduMultiplierEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DspFertilityMultiplier"/> class.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        public DspFertilityMultiplier(params MsfDataSourcePart[] inputs)
            : base(inputs) { }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.DspFertilityMultiplier;

        /// <summary>
        /// Generates the data.
        /// </summary>
        protected override void GenerateData()
        {
            var Fertility = GetInputDataOfType<FertilityCompleteBaseEntity>();

            var baseData = Fertility.Where(m => (m.Education != Education.Total || m.BirthOrder != BirthOrder.Total));
            var totalData = Fertility.Where(m => !(m.Education != Education.Total || m.BirthOrder != BirthOrder.Total));

            var FertilityMultipliers = baseData
                .Join(totalData,
                e => new { e.Year, e.Age, e.Gender },
                t => new { t.Year, t.Age, t.Gender },
                (e, t) => new FertilityEduMultiplierEntity
                {
                    Year = e.Year,
                    Age = e.Age,
                    Gender = e.Gender,
                    Education = e.Education,
                    BirthOrder = e.BirthOrder,
                    Value = t.Value == 0 ? 1 : e.Value / t.Value
                });

            Data = FertilityMultipliers;
        }     
    }
}
