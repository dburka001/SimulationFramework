using MicroSim.DataSource.Core;
using MicroSim.DataSource.Entities;
using MicroSim.DataSource.DataAccess;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.Population
{
    /// <summary>
    /// DspPopulationRaw class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSourcePart" />
    public class DspPopulationRaw : MsfDataSourcePart<PopulationRawEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DspPopulationEduRaw" /> class.
        /// </summary>
        /// <param name="inputPath">Name of the input file.</param>
        public DspPopulationRaw(string inputPath) 
            : base(inputPath) { }            

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.DspPopulationRaw;
    }
}
