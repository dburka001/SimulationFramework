using MicroSim.DataSource.Core;
using MicroSim.DataSource.Entities;
using MicroSim.DataSource.Population;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.PopulationEdu
{
    /// <summary>
    /// PopulationEduDataSource class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSource" />
    public class PopulationEduDataSource : MsfDataSource
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.DsPopulationEdu;

        /// <summary>
        /// Initializes a new instance of the <see cref="PopulationEduDataSource"/> class.
        /// </summary>
        public PopulationEduDataSource(string inputFileName, params MsfDataSource[] inputs) 
            : base(inputs)
        {
            AddPart(new DspPopulationEduRaw(
                GetSourcePath(inputFileName)));            
            AddPart(new DspPopulationEduBase(
                GetPartOfType<DspPopulationEduRaw>()));
            AddPart(new DspPopulationEduYearly(
                GetPartOfType<DspPopulationEduBase>(),
                GetInputDataOfType<PopulationDataSource>().GetPartOfType<DspPopulationExt>()));
            AddPart(new DspPopulationEduAgeTree(
                GetPartOfType<DspPopulationEduYearly>()));
        }
    }
}
