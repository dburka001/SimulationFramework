using MicroSim.DataSource.Core;
using MicroSim.DataSource.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.BirthEdu
{
    /// <summary>
    /// BirthEduDataSource class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSource" />
    public class BirthEduDataSource : MsfDataSource
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.DsBirthEdu;

        /// <summary>
        /// Initializes a new instance of the <see cref="BirthEduDataSource"/> class.
        /// </summary>
        public BirthEduDataSource(string inputFileName, params MsfDataSource[] inputs)
            : base(inputs)
        {
            AddPart(new DspBirthEduRaw(
                GetSourcePath(inputFileName)));
            AddPart(new DspBirthEduBase(
                GetPartOfType<DspBirthEduRaw>()));
            AddPart(new DspBirthEduAgeTree(
                GetPartOfType<DspBirthEduBase>()));
        }
    }
}
