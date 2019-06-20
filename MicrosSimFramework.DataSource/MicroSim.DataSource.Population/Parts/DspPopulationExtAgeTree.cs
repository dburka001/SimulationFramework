using MicroSim.DataSource.Core;
using MicroSim.DataSource.Entities;
using MicroSim.DataSource.DataAccess;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MicroSim.DataSource.Population
{
    /// <summary>
    /// DspPopulationExtAgeTree class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSourcePart" />
    public class DspPopulationExtAgeTree : MsfDataSourcePart<PopulationBaseEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DspPopulationExtAgeTree"/> class.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        public DspPopulationExtAgeTree(params MsfDataSourcePart[] inputs)
            : base(inputs) { }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.DspPopulationExtAgeTree;

        /// <summary>
        /// Gets the view.
        /// </summary>
        /// <value>
        /// The view.
        /// </value>
        public override UserControl View { get; } = new MsfAgeTreeDisplayUC();

        /// <summary>
        /// Generates the data.
        /// </summary>
        protected override void GenerateData()
        {
            var populationAgeTree = new List<PopulationBaseEntity>();
            var population = GetInputDataOfType<PopulationBaseEntity>();            

            foreach (var p in population)
            {
                var pat = new PopulationBaseEntity()
                {
                    Age = p.Age,
                    Gender = p.Gender,
                    Year = p.Year,
                    Value = p.Value
                };
                populationAgeTree.Add(pat);
            }
            Data = populationAgeTree;
        }
    }
}
