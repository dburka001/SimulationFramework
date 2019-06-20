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
    /// DspFertilityAgeTree class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSourcePart{MicroSim.DataSource.Entities.FertilityCompleteBaseEntity}" />
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSourcePart" />
    public class DspFertilityAgeTree : MsfDataSourcePart<FertilityCompleteBaseEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DspFertilityAgeTree"/> class.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        public DspFertilityAgeTree(params MsfDataSourcePart[] inputs)
            : base(inputs) { }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.DspFertilityAgeTree;

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
            var FertilityRawAgeTree = new List<FertilityCompleteBaseEntity>();
            var FertilityRaw = GetInputDataOfType<FertilityCompleteBaseEntity>();

            foreach (var f in FertilityRaw)
            {
                FertilityRawAgeTree.Add(new FertilityCompleteBaseEntity()
                {
                    Age = f.Age,
                    DeathCount = f.DeathCount,
                    Education = f.Education,
                    BirthOrder = f.BirthOrder,
                    Gender = f.Gender,
                    Population = f.Population,
                    Value = f.Value,
                    Year = f.Year
                });
            }

            Data = FertilityRaw;
        }
    }
}
