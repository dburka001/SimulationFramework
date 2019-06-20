using MicroSim.DataSource.Core;
using MicroSim.DataSource.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MicroSim.DataSource.Death
{
    /// <summary>
    /// DspDeathAgeTree class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSourcePart" />
    public class DspDeathAgeTree : MsfDataSourcePart<DeathBaseEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DspDeathAgeTree"/> class.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        public DspDeathAgeTree(params MsfDataSourcePart[] inputs)
            : base(inputs) { }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.DspDeathAgeTree;

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
            var DeathAgeTree = new List<DeathBaseEntity>();
            var Death = GetInputDataOfType<DeathBaseEntity>();            

            foreach (var p in Death)
            {
                var pat = new DeathBaseEntity()
                {
                    Age = p.Age,
                    Gender = p.Gender,
                    Year = p.Year,
                    Value = p.Value
                };
                DeathAgeTree.Add(pat);
            }
            Data = DeathAgeTree;
        }
    }
}
