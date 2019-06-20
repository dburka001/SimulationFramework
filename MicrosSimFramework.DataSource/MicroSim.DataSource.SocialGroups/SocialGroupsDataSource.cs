
using MicroSim.DataSource.Core;
using MicroSim.DataSource.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.SocialGroups
{
    /// <summary>
    /// SocialGroupsDataSource class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSource" />
    public class SocialGroupsDataSource : MsfDataSource
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.SocialGroupsDataSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="StartingPopulationDataSource"/> class.
        /// </summary>
        public SocialGroupsDataSource(string[] inputFileNames, params MsfDataSource[] inputs)
            : base(inputs)
        {
            AddPart(new DspMinorityRates(
                GetSourcePath(inputFileNames[0])));
            AddPart(new DspImmigrantSettings(
                GetSourcePath(inputFileNames[1])));            
        }
    }
}
