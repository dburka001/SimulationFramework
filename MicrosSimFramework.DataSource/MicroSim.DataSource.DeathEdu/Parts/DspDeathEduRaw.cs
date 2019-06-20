using MicroSim.DataSource.Core;
using MicroSim.DataSource.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.DeathEdu
{
    /// <summary>
    /// DspDeathEduRaw class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSourcePart{MicroSim.DataSource.Entities.DeathEduRawEntity}" />
    public class DspDeathEduRaw : MsfDataSourcePart<DeathEduRawEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DspDeathEduRaw"/> class.
        /// </summary>
        /// <param name="inputPath">The input path.</param>
        public DspDeathEduRaw(string inputPath)
            : base(inputPath) { }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.DspDeathEduRaw;
    }
}
