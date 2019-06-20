using MicroSim.DataSource.Core;
using MicroSim.DataSource.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MicroSim.DataSource.BirthComplete
{
    /// <summary>
    /// DspBirthCompleteAgeTree class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSourcePart" />
    public class DspBirthCompleteAgeTree : MsfDataSourcePart<BirthCompleteEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DspBirthCompleteAgeTree"/> class.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        public DspBirthCompleteAgeTree(params MsfDataSourcePart[] inputs)
            : base(inputs)
        { }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.DspBirthCompleteAgeTree;

        /// <summary>
        /// Gets the view.
        /// </summary>
        /// <value>
        /// The view.
        /// </value>
        public override UserControl View { get; } = new MsfAgeTreeCompleteDisplayUC();

        /// <summary>
        /// Generates the data.
        /// </summary>
        protected override void GenerateData()
        {
            var birthMotherAgeTree = new List<BirthCompleteEntity>();
            var birthMothers = GetInputDataOfType<BirthCompleteEntity>();            

            foreach (var d in birthMothers)
            {
                var dat = new BirthCompleteEntity()
                {
                    Age = d.Age,
                    Gender = d.Gender,
                    Education = d.Education,
                    Year = d.Year,
                    BirthOrder = d.BirthOrder,
                    Value = d.Value
                };
                birthMotherAgeTree.Add(dat);
            }
            Data = birthMotherAgeTree;
        }
    }
}
