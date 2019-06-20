using MicroSim.DataSource.BirthEdu;
using MicroSim.DataSource.Core;
using MicroSim.DataSource.Entities;
using MicroSim.DataSource.PopulationEdu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.BirthComplete
{
    /// <summary>
    /// BirthCompleteDataSource class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSource" />
    public class BirthCompleteDataSource : MsfDataSource
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.DsBirthComplete;

        /// <summary>
        /// Initializes a new instance of the <see cref="BirthCompleteDataSource"/> class.
        /// </summary>
        public BirthCompleteDataSource(string[] inputFileNames, params MsfDataSource[] inputs)
            : base(inputs)
        {
            AddPart(new DspBirthMotherRaw(
                GetSourcePath(inputFileNames[0])));
            AddPart(new DspBirthMother(
                GetPartOfType<DspBirthMotherRaw>(),
                GetInputDataOfType<PopulationEduDataSource>().GetPartOfType<DspPopulationEduYearly>()));
            AddPart(new DspBirthMotherAgeTree(
                GetPartOfType<DspBirthMother>()));
            AddPart(new DspBirthCompleteRaw(
                GetSourcePath(inputFileNames[1])));
            AddPart(new DspBirthComplete(
                GetPartOfType<DspBirthCompleteRaw>(),
                GetInputDataOfType<BirthEduDataSource>().GetPartOfType<DspBirthEduBase>()));
            AddPart(new DspBirthCompleteAgeTree(
                GetPartOfType<DspBirthComplete>()));
        }
    }
}
