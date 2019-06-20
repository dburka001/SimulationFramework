using MicroSim.DataSource.Core;
using MicroSim.DataSource.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MicroSim.DataSource.DeathEdu
{
    /// <summary>
    /// DspDeathEduAgeTree class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSourcePart" />
    public class DspDeathEduAgeTree : MsfDataSourcePart<DeathEduBaseEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DspPopulationEduAgeTree"/> class.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        public DspDeathEduAgeTree(params MsfDataSourcePart[] inputs)
            : base(inputs)
        { }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.DspDeathEduAgeTree;

        /// <summary>
        /// Gets the view.
        /// </summary>
        /// <value>
        /// The view.
        /// </value>
        public override UserControl View { get; } = new MsfAgeTreeEduDisplayUC();

        /// <summary>
        /// Generates the data.
        /// </summary>
        protected override void GenerateData()
        {
            var deathEduAgeTree = new List<DeathEduBaseEntity>();
            var deathEduBase = GetInputDataOfType<DeathEduBaseEntity>();            

            foreach (var d in deathEduBase)
            {
                var dat = new DeathEduBaseEntity()
                {
                    Age = d.Age,
                    Gender = d.Gender,
                    Education = d.Education,
                    Year = d.Year,
                    Value = d.Value
                };
                deathEduAgeTree.Add(dat);
            }
            Data = deathEduAgeTree;
        }
    }
}
