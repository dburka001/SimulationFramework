using MicroSim.DataSource.Core;
using MicroSim.DataSource.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.Death
{
    /// <summary>
    /// DeathDataSource class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSource" />
    public class DeathDataSource : MsfDataSource
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.DsDeath;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeathDataSource"/> class.
        /// </summary>
        public DeathDataSource(string inputFileName)
        {
            var DeathRaw = new DspDeathRaw(GetSourcePath(inputFileName));
            AddPart(DeathRaw);

            var DeathBase = new DspDeathBase(DeathRaw);
            AddPart(DeathBase);

            var DeathAgeTree = new DspDeathAgeTree(DeathBase);
            AddPart(DeathAgeTree);
        }
    }
}
