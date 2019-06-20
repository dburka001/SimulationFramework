
using MicroSim.DataSource.Core;
using MicroSim.DataSource.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.Other
{
    /// <summary>
    /// OtherDataSource class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSource" />
    public class OtherDataSource : MsfDataSource
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.DsOther;

        /// <summary>
        /// Initializes a new instance of the <see cref="PopulationEduDataSource"/> class.
        /// </summary>
        public OtherDataSource(string[] inputFileNames, params MsfDataSource[] inputs)
            : base(inputs)
        {
            AddPart(new DspEduBasedVar(
                GetSourcePath(inputFileNames[0])));
            AddPart(new DspIncomeIncrease(
                GetSourcePath(inputFileNames[1])));
            AddPart(new DspIncomeIncreaseGraphs(
                GetPartOfType<DspIncomeIncrease>(),
                GetPartOfType<DspEduBasedVar>()));
            AddPart(new DspWorkStatusRaw(
                GetSourcePath(inputFileNames[2])));
            AddPart(new DspWorkStatus(
                GetPartOfType<DspWorkStatusRaw>(),
                GetPartOfType<DspEduBasedVar>()));
            AddPart(new DspWorkStatusTime(
                GetSourcePath(inputFileNames[3])));
            AddPart(new DspPensionMultiplier(
                GetSourcePath(inputFileNames[4])));
            AddPart(new DspSocialGroupEducationType(
                GetSourcePath(inputFileNames[5])));
            AddPart(new DspPensionBasedVar(
                GetSourcePath(inputFileNames[6])));
            AddPart(new DspEconomicGrowth(
                GetSourcePath(inputFileNames[7])));
        }
    }
}
