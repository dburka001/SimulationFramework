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
    /// DspEconomicGrowth class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSourcePart" />
    public class DspEconomicGrowth : MsfDataSourcePart<EconomicGrowthEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DspEconomicGrowth"/> class.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        public DspEconomicGrowth(string inputPath) 
            : base(inputPath) { }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.DspEconomicGrowth;
    }
}
