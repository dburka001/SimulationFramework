using MicroSim.DataSource.Core;
using MicroSim.DataSource.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.Birth
{
    /// <summary>
    /// BirthDataSource class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSource" />
    public class BirthDataSource : MsfDataSource
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.DsBirth;

        /// <summary>
        /// Initializes a new instance of the <see cref="BirthDataSource"/> class.
        /// </summary>
        public BirthDataSource(string inputFileName)
        {
            var BirthRaw = new DspBirthRaw(GetSourcePath(inputFileName));
            AddPart(BirthRaw);

            var BirthBase = new DspBirthBase(BirthRaw);
            AddPart(BirthBase);

            var BirthAgeTree = new DspBirthAgeTree(BirthBase);
            AddPart(BirthAgeTree);
        }
    }
}
