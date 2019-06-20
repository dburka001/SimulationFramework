using MicroSim.DataSource.Core;
using MicroSim.DataSource.Entities;
using MicroSim.DataSource.DataAccess;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.Population
{
    /// <summary>
    /// DspPopulationBase class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSourcePart" />
    public class DspPopulationBase : MsfDataSourcePart<PopulationBaseEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DspPopulationBase"/> class.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        public DspPopulationBase(params MsfDataSourcePart[] inputs) 
            : base(inputs) { }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.DspPopulationBase;

        /// <summary>
        /// Generates the data.
        /// </summary>
        protected override void GenerateData()
        {
            var population = new List<PopulationBaseEntity>();
            var populationRaw = GetInputDataOfType<PopulationRawEntity>();            

            foreach (var pr in populationRaw)
            {
                var pb = new PopulationBaseEntity();

                pb.Gender = GenderExtensions.Parse(pr.Sex);
                if (pb.Gender.IsFiltered())
                    continue;                
                pb.Age = Convert.ToInt32(pr.Age);
                pb.Year = pr.Year;
                pb.Value = pr.Value;
                population.Add(pb);
            }
            Data = population;
        }
    }
}
