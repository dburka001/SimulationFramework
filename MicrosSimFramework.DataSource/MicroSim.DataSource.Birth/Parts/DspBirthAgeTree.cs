using MicroSim.DataSource.Core;
using MicroSim.DataSource.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MicroSim.DataSource.Birth
{
    /// <summary>
    /// DspBirthAgeTree class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSourcePart" />
    public class DspBirthAgeTree : MsfDataSourcePart<BirthBaseEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DspBirthAgeTree"/> class.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        public DspBirthAgeTree(params MsfDataSourcePart[] inputs)
            : base(inputs) { }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.DspBirthAgeTree;

        /// <summary>
        /// Gets the view.
        /// </summary>
        /// <value>
        /// The view.
        /// </value>
        public override UserControl View { get; } = new MsfAgeTreeBoDisplayUC();

        /// <summary>
        /// Generates the data.
        /// </summary>
        protected override void GenerateData()
        {
            var BirthAgeTree = new List<BirthBaseEntity>();
            var Birth = GetInputDataOfType<BirthBaseEntity>();            

            foreach (var p in Birth)
            {
                var pat = new BirthBaseEntity()
                {
                    Age = p.Age,
                    Gender = p.Gender,
                    BirthOrder = p.BirthOrder,
                    Year = p.Year,
                    Value = p.Value
                };
                BirthAgeTree.Add(pat);
            }
            Data = BirthAgeTree;
        }
    }
}
