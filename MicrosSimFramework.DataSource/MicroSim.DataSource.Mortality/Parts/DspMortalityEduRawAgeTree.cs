using MicroSim.DataSource.Core;
using MicroSim.DataSource.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MicroSim.DataSource.Mortality
{
    /// <summary>
    /// DspMortalityEduRawAgeTree class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSourcePart{MicroSim.DataSource.Entities.MortalityEduBaseEntity}" />
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSourcePart" />
    public class DspMortalityEduRawAgeTree : MsfDataSourcePart<MortalityEduBaseEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DspMortalityEduRawAgeTree"/> class.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        public DspMortalityEduRawAgeTree(params MsfDataSourcePart[] inputs)
            : base(inputs) { }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.DspMortalityEduRawAgeTree;

        /// <summary>
        /// Gets the view.
        /// </summary>
        /// <value>
        /// The view.
        /// </value>
        public override UserControl View { get; } = new MsfAgeTreeEduDisplayUC() { CreateTotal = false };

        /// <summary>
        /// Generates the data.
        /// </summary>
        protected override void GenerateData()
        {
            var mortalityRawAgeTree = new List<MortalityEduBaseEntity>();
            var mortalityRaw = GetInputDataOfType<MortalityEduBaseEntity>();

            foreach (var m in mortalityRaw)
            {
                mortalityRawAgeTree.Add(new MortalityEduBaseEntity()
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

            Data = mortalityRaw;
        }
    }
}
