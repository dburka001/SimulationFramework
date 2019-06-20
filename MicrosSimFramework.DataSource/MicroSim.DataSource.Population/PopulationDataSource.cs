
using MicroSim.DataSource.Core;
using MicroSim.DataSource.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.Population
{
    /// <summary>
    /// PopulationDataSource class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSource" />
    public class PopulationDataSource : MsfDataSource
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.DsPopulation;

        /// <summary>
        /// Initializes a new instance of the <see cref="PopulationEduDataSource"/> class.
        /// </summary>
        public PopulationDataSource(string inputFileName)
        {
            var populationRaw = new DspPopulationRaw(GetSourcePath(inputFileName));
            AddPart(populationRaw);

            var populationBase = new DspPopulationBase(populationRaw);
            AddPart(populationBase);

            var populationAgeTree = new DspPopulationAgeTree(populationBase);
            AddPart(populationAgeTree);

            var populationExt = new DspPopulationExt(populationBase);
            AddPart(populationExt);

            var populationExtAgeTree = new DspPopulationExtAgeTree(populationExt);
            AddPart(populationExtAgeTree);
        }
    }
}
