using MicroSim.DataSource.Core;
using MicroSim.DataSource.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.Birth
{
    /// <summary>
    /// DspBirthRaw class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSourcePart" />
    public class DspBirthRaw : MsfDataSourcePart<BirthRawEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DspBirthEduRaw" /> class.
        /// </summary>
        /// <param name="inputPath">Name of the input file.</param>
        public DspBirthRaw(string inputPath) 
            : base(inputPath) { }            

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.DspBirthRaw;
    }
}
