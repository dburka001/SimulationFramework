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
    /// DspDeathEduExtAgeTree class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSourcePart" />
    public class DspDeathEduExtAgeTree : MsfDataSourcePart<DeathEduBaseEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DspDeathEduExtAgeTree"/> class.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        public DspDeathEduExtAgeTree(params MsfDataSourcePart[] inputs)
            : base(inputs)
        { }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.DspDeathEduExtAgeTree;

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
            var deathEduExtAgeTree = new List<DeathEduBaseEntity>();
            var deathEduExt = GetInputDataOfType<DeathEduBaseEntity>();            

            foreach (var d in deathEduExt)
            {
                var dat = new DeathEduBaseEntity()
                {
                    Age = d.Age,
                    Gender = d.Gender,
                    Education = d.Education,
                    Year = d.Year,
                    Value = d.Value
                };
                deathEduExtAgeTree.Add(dat);
            }

            Data = deathEduExtAgeTree;
        }
    }
}
