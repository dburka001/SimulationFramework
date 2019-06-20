using MicroSim.DataSource.Core;
using MicroSim.DataSource.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.BirthComplete
{
    /// <summary>
    /// DspBirthMotherRaw class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSourcePart{MicroSim.DataSource.Entities.BirthMotherRawEntity}" />
    public class DspBirthMotherRaw : MsfDataSourcePart<BirthMotherRawEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DspBirthMotherRaw"/> class.
        /// </summary>
        /// <param name="inputPath">The input path.</param>
        public DspBirthMotherRaw(string inputPath)
            : base(inputPath) { }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.DspBirthMotherRaw;
    }
}
