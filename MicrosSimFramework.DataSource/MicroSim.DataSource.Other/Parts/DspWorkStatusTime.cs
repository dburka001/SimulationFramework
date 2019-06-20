using MicroSim.DataSource.Core;
using MicroSim.DataSource.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.Other
{
    /// <summary>
    /// DspWorkStatusTime class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSourcePart" />
    public class DspWorkStatusTime : MsfDataSourcePart<WorkStatusTimeEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DspWorkStatusTime"/> class.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        public DspWorkStatusTime(string inputPath) 
            : base(inputPath) { }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.DspWorkStatusTime;
    }
}
