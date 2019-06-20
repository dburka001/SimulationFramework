using MicroSim.DataSource.Core;
using MicroSim.DataSource.DataAccess;
using MicroSim.DataSource.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.DeterministicEducation
{
    /// <summary>
    /// DspDetEduRaw class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSourcePart{MicroSim.DataSource.DeterministicEducation.DetEduRawEntity}" />
    public class DspDetEduRaw : MsfDataSourcePart<DetEduRawEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DspPopulationEduRaw"/> class.
        /// </summary>
        /// <param name="inputPath">The input path.</param>
        public DspDetEduRaw(string inputPath)
            : base(inputPath) { }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.DspDetEduRaw;
    }
}
