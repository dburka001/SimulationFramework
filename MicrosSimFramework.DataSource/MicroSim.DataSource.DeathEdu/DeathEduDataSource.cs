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
    /// DeathEduDataSource class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSource" />
    public class DeathEduDataSource : MsfDataSource
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.DsDeathEdu;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeathEduDataSource"/> class.
        /// </summary>
        public DeathEduDataSource(string inputFileName, params MsfDataSource[] inputs)
            : base(inputs)
        {
            AddPart(new DspDeathEduRaw(
                GetSourcePath(inputFileName)));
            AddPart(new DspDeathEduBase(
                GetPartOfType<DspDeathEduRaw>()));
            AddPart(new DspDeathEduAgeTree(
                GetPartOfType<DspDeathEduBase>()));
            AddPart(new DspDeathEduExt(
                GetPartOfType<DspDeathEduBase>()));
            AddPart(new DspDeathEduExtAgeTree(
                GetPartOfType<DspDeathEduExt>()));
        }
    }
}
