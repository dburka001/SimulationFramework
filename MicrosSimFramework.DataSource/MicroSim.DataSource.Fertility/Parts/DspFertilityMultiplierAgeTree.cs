using MicroSim.DataSource.Core;
using MicroSim.DataSource.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MicroSim.DataSource.Fertility
{
    /// <summary>
    /// DspFertilityMultiplierAgeTree class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSourcePart{MicroSim.DataSource.Entities.FertilityCompleteBaseEntity}" />
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSourcePart" />
    public class DspFertilityMultiplierAgeTree : MsfDataSourcePart<FertilityEduMultiplierEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DspFertilityMultiplierAgeTree"/> class.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        public DspFertilityMultiplierAgeTree(params MsfDataSourcePart[] inputs)
            : base(inputs) { }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.DspFertilityMultiplierAgeTree;

        /// <summary>
        /// Gets the view.
        /// </summary>
        /// <value>
        /// The view.
        /// </value>
        public override UserControl View { get; } = new MsfAgeTreeCompleteDisplayUC() { CreateTotal = false };

        /// <summary>
        /// Generates the data.
        /// </summary>
        protected override void GenerateData()
        {
            var FertilityRawAgeTree = new List<FertilityEduMultiplierEntity>();
            var FertilityRaw = GetInputDataOfType<FertilityEduMultiplierEntity>();

            foreach (var f in FertilityRaw)
            {
                FertilityRawAgeTree.Add(new FertilityEduMultiplierEntity()
                {
                    Age = f.Age,
                    Education = f.Education,
                    BirthOrder = f.BirthOrder,
                    Gender = f.Gender,
                    Value = f.Value,
                    Year = f.Year
                });
            }

            Data = FertilityRaw;
        }
    }
}
