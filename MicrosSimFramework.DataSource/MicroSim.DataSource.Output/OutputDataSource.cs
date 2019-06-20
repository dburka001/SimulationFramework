
using MicroSim.DataSource.Core;
using MicroSim.DataSource.DeterministicEducation;
using MicroSim.DataSource.Entities;
using MicroSim.DataSource.Fertility;
using MicroSim.DataSource.Mortality;
using MicroSim.DataSource.Other;
using MicroSim.DataSource.StartingPopulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.Output
{
    /// <summary>
    /// OutputDataSource class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSource" />
    public class OutputDataSource : MsfDataSource
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.DsOutput;

        /// <summary>
        /// Initializes a new instance of the <see cref="PopulationEduDataSource"/> class.
        /// </summary>
        public OutputDataSource(params MsfDataSource[] inputs)
            : base(inputs)
        {
            AddPart(new DspOutputPopulationValidation(
                GetInputDataOfType<StartingPopulationDataSource>().GetPartOfType<DspStartValidation>()));
            AddPart(new DspOutputPopulation(
                GetInputDataOfType<StartingPopulationDataSource>().GetPartOfType<DspStartWithMinoritiesAndMigrants>()));
            AddPart(new DspOutputMortality(
                GetInputDataOfType<MortalityDataSource>().GetPartOfType<DspMortalityEduForecast>()));
            AddPart(new DspOutputFertilityTotal(
                GetInputDataOfType<FertilityDataSource>().GetPartOfType<DspFertilityForecast>()));
            AddPart(new DspOutputFertility(
                GetInputDataOfType<FertilityDataSource>().GetPartOfType<DspFertilityForecast>()));
            AddPart(new DspOutputEducation(
                GetInputDataOfType<DetEduDataSource>().GetPartOfType<DspDetEdu>()));
            AddPart(new DspOutputWorkYearStart(
                GetInputDataOfType<OtherDataSource>().GetPartOfType<DspEduBasedVar>()));
            AddPart(new DspOutputStartingIncome(
                GetInputDataOfType<OtherDataSource>().GetPartOfType<DspEduBasedVar>()));
            AddPart(new DspOutputIncomeIncrease(
                GetInputDataOfType<OtherDataSource>().GetPartOfType<DspIncomeIncrease>()));
            AddPart(new DspOutputWorkStatus(
                GetInputDataOfType<OtherDataSource>().GetPartOfType<DspWorkStatus>()));
            AddPart(new DspOutputWorkStatusMinTime(
                GetInputDataOfType<OtherDataSource>().GetPartOfType<DspWorkStatusTime>()));
            AddPart(new DspOutputWorkStatusMaxTime(
                GetInputDataOfType<OtherDataSource>().GetPartOfType<DspWorkStatusTime>()));
            AddPart(new DspOutputPensionMultiplier(
                GetInputDataOfType<OtherDataSource>().GetPartOfType<DspPensionMultiplier>()));
            AddPart(new DspOutputPensionBase(
                GetInputDataOfType<OtherDataSource>().GetPartOfType<DspPensionBasedVar>()));
            AddPart(new DspOutputPensionTheta(
                GetInputDataOfType<OtherDataSource>().GetPartOfType<DspPensionBasedVar>()));
            AddPart(new DspOutputSocialGroupEducationType(
                GetInputDataOfType<OtherDataSource>().GetPartOfType<DspSocialGroupEducationType>()));
            AddPart(new DspOutputEconomicGrowth(
                GetInputDataOfType<OtherDataSource>().GetPartOfType<DspEconomicGrowth>()));
        }
    }
}
