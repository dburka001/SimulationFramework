
using MicroSim.DataSource.BirthComplete;
using MicroSim.DataSource.Core;
using MicroSim.DataSource.Entities;
using MicroSim.DataSource.Population;
using MicroSim.DataSource.PopulationEdu;
using MicroSim.DataSource.SocialGroups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.StartingPopulation
{
    /// <summary>
    /// OutputDataSource class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSource" />
    public class StartingPopulationDataSource : MsfDataSource
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.DsStartingPopulation;

        /// <summary>
        /// Initializes a new instance of the <see cref="StartingPopulationDataSource"/> class.
        /// </summary>
        public StartingPopulationDataSource(params MsfDataSource[] inputs)
            : base(inputs)
        {
            AddPart(new DspStartValidation(
               GetInputDataOfType<PopulationDataSource>().GetPartOfType<DspPopulationExt>()));
            AddPart(new DspStartRaw(
                GetInputDataOfType<PopulationEduDataSource>().GetPartOfType<DspPopulationEduYearly>()));
            AddPart(new DspStartComplete(
                GetPartOfType<DspStartRaw>(),
                GetInputDataOfType<BirthCompleteDataSource>().GetPartOfType<DspBirthMother>()));
            AddPart(new DspStartWithMinorities(
                GetPartOfType<DspStartComplete>(),
                GetInputDataOfType<SocialGroupsDataSource>().GetPartOfType<DspMinorityRates>()));
            AddPart(new DspStartWithMinoritiesAndMigrants(
                GetPartOfType<DspStartWithMinorities>(),
                GetInputDataOfType<SocialGroupsDataSource>().GetPartOfType<DspImmigrantSettings>()));
        }
    }
}
