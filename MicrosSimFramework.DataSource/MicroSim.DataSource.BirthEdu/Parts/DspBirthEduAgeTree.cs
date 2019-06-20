using MicroSim.DataSource.Core;
using MicroSim.DataSource.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MicroSim.DataSource.BirthEdu
{
    /// <summary>
    /// DspBirthEduAgeTree class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSourcePart" />
    public class DspBirthEduAgeTree : MsfDataSourcePart<BirthEduBaseEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DspBirthEduAgeTree"/> class.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        public DspBirthEduAgeTree(params MsfDataSourcePart[] inputs)
            : base(inputs)
        { }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.DspBirthEduAgeTree;

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
            var BirthEduAgeTree = new List<BirthEduBaseEntity>();
            var BirthEduBase = GetInputDataOfType<BirthEduBaseEntity>();            

            foreach (var d in BirthEduBase)
            {
                var dat = new BirthEduBaseEntity()
                {
                    Age = d.Age,
                    Gender = d.Gender,
                    Education = d.Education,
                    Year = d.Year,
                    Value = d.Value
                };
                BirthEduAgeTree.Add(dat);
            }
            Data = BirthEduAgeTree;
        }
    }
}
