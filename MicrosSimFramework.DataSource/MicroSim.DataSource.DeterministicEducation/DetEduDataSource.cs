using MicroSim.DataSource.Core;
using MicroSim.DataSource.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.DeterministicEducation
{
    /// <summary>
    /// DetEduDataSource class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSource" />
    public class DetEduDataSource : MsfDataSource
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.DsDetEdu;

        /// <summary>
        /// Initializes a new instance of the <see cref="PopulationEduDataSource"/> class.
        /// </summary>
        public DetEduDataSource(string inputFileName, params MsfDataSource[] inputs)
            : base(inputs)
        {
            AddPart(new DspDetEduRaw(
                GetSourcePath(inputFileName)));
            AddPart(new DspDetEdu(
                GetPartOfType<DspDetEduRaw>()));
        }
    }
}
