using MicroSim.DataSource.Birth;
using MicroSim.DataSource.BirthEdu;
using MicroSim.DataSource.Core;
using MicroSim.DataSource.Death;
using MicroSim.DataSource.DeathEdu;
using MicroSim.DataSource.Entities;
using MicroSim.DataSource.Population;
using MicroSim.DataSource.PopulationEdu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.Mortality
{
    /// <summary>
    /// MortalityDataSource class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSource" />
    public class MortalityDataSource : MsfDataSource
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.DsMortality;

        /// <summary>
        /// Initializes a new instance of the <see cref="PopulationEduDataSource"/> class.
        /// </summary>
        public MortalityDataSource(params MsfDataSource[] inputs)
            : base(inputs)
        {
            AddPart(new DspMortalityEduRaw(
                GetInputDataOfType<PopulationDataSource>().GetPartOfType<DspPopulationExt>(),
                GetInputDataOfType<DeathDataSource>().GetPartOfType<DspDeathBase>(),
                GetInputDataOfType<BirthDataSource>().GetPartOfType<DspBirthBase>(),
                GetInputDataOfType<PopulationEduDataSource>().GetPartOfType<DspPopulationEduYearly>(),
                GetInputDataOfType<DeathEduDataSource>().GetPartOfType<DspDeathEduExt>(),
                GetInputDataOfType<BirthEduDataSource>().GetPartOfType<DspBirthEduBase>()));
            AddPart(new DspMortalityEduRawAgeTree(
               GetPartOfType<DspMortalityEduRaw>()));
            AddPart(new DspMortalityEduBase(
                GetPartOfType<DspMortalityEduRaw>()));
            AddPart(new DspMortalityEduAgeTree(
                GetPartOfType<DspMortalityEduBase>()));
            AddPart(new DspMortalityEduMultiplier(
               GetPartOfType<DspMortalityEduBase>()));
            AddPart(new DspMortalityEduMultiplierAgeTree(
                GetPartOfType<DspMortalityEduMultiplier>()));
            AddPart(new DspMortalityEduForecast(
                GetPartOfType<DspMortalityEduBase>(),
                GetPartOfType<DspMortalityEduMultiplier>()));
            AddPart(new DspMortalityEduForecastAgeTree(               
                GetPartOfType<DspMortalityEduForecast>()));
        }
    }
}
