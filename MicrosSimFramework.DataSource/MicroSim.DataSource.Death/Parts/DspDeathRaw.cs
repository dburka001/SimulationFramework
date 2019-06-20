using MicroSim.DataSource.Core;
using MicroSim.DataSource.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.Death
{
    /// <summary>
    /// DspDeathRaw class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSourcePart" />
    public class DspDeathRaw : MsfDataSourcePart<DeathRawEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DspDeathEduRaw" /> class.
        /// </summary>
        /// <param name="inputPath">Name of the input file.</param>
        public DspDeathRaw(string inputPath) 
            : base(inputPath) { }            

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.DspDeathRaw;
    }
}
