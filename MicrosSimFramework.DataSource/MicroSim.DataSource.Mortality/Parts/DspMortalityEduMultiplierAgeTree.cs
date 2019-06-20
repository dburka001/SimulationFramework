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
    /// DspMortalityEduMultiplierAgeTree class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSourcePart{MicroSim.DataSource.Entities.MortalityEduBaseEntity}" />
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSourcePart" />
    public class DspMortalityEduMultiplierAgeTree : MsfDataSourcePart<MortalityEduMultiplierEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DspMortalityEduMultiplierAgeTree"/> class.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        public DspMortalityEduMultiplierAgeTree(params MsfDataSourcePart[] inputs)
            : base(inputs) { }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.DspMortalityEduMultiplierAgeTree;

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
            var mortalityMultiplierAgeTree = new List<MortalityEduMultiplierEntity>();
            var mortalityMultiplier = GetInputDataOfType<MortalityEduMultiplierEntity>();

            foreach (var m in mortalityMultiplier)
            {
                mortalityMultiplierAgeTree.Add(new MortalityEduMultiplierEntity()
                {
                    Age = m.Age,
                    Education = m.Education,
                    Gender = m.Gender,
                    Value = m.Value,
                    Year = m.Year
                });
            }

            Data = mortalityMultiplierAgeTree;
        }
    }
}
