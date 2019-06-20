using MicroSim.DataSource.Birth;
using MicroSim.DataSource.BirthComplete;
using MicroSim.DataSource.Core;
using MicroSim.DataSource.Entities;
using MicroSim.DataSource.Population;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.Fertility
{
    /// <summary>
    /// FertilityDataSource class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSource" />
    public class FertilityDataSource : MsfDataSource
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.DsFertility;

        /// <summary>
        /// Initializes a new instance of the <see cref="PopulationEduDataSource"/> class.
        /// </summary>
        public FertilityDataSource(params MsfDataSource[] inputs)
            : base(inputs)
        {
            AddPart(new DspFertilityRaw(
                GetInputDataOfType<PopulationDataSource>().GetPartOfType<DspPopulationExt>(),
                GetInputDataOfType<BirthDataSource>().GetPartOfType<DspBirthBase>(),
                GetInputDataOfType<BirthCompleteDataSource>().GetPartOfType<DspBirthMother>(),
                GetInputDataOfType<BirthCompleteDataSource>().GetPartOfType<DspBirthComplete>()));
            AddPart(new DspFertilityRawAgeTree(
               GetPartOfType<DspFertilityRaw>()));
            AddPart(new DspFertilityBase(
                GetPartOfType<DspFertilityRaw>()));
            AddPart(new DspFertilityAgeTree(
               GetPartOfType<DspFertilityBase>()));
            AddPart(new DspFertilityMultiplier(
               GetPartOfType<DspFertilityBase>()));
            AddPart(new DspFertilityMultiplierAgeTree(
               GetPartOfType<DspFertilityMultiplier>()));
            AddPart(new DspFertilityForecast(
               GetPartOfType<DspFertilityBase>(),
               GetPartOfType<DspFertilityMultiplier>()));
            AddPart(new DspFertilityForecastAgeTree(
               GetPartOfType<DspFertilityForecast>()));
        }
    }
}
