using MicroSim.DataSource.Core;
using MicroSim.DataSource.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.Output
{
    /// <summary>
    /// DspOutputFertility class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSourcePart" />
    public class DspOutputFertility : MsfDataSourcePart<OutputFertilityEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DspOutputFertilityTotal"/> class.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        public DspOutputFertility(params MsfDataSourcePart[] inputs) 
            : base(inputs) { }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.DspOutputFertility;

        /// <summary>
        /// Generates the data.
        /// </summary>
        protected override void GenerateData()
        {            
            var output = new List<OutputFertilityEntity>();
            var data = GetInputDataOfType<FertilityCompleteBaseEntity>()
                .Where(d => d.Education != Education.Total && d.BirthOrder != BirthOrder.Total);

            foreach (var d in data)
            {
                output.Add(new OutputFertilityEntity()
                {
                    Age = d.Age,      
                    Education = d.Education,
                    BirthOrder = (byte)((int)d.BirthOrder - 2),
                    Value = d.Value,
                    Year = d.Year
                });
            }

            Data = output
                .Where(o => o.Year >= Settings.StartYear)
                .OrderBy(o => o.Year)
                .ThenBy(o => o.Age)
                .ThenBy(o => o.Education)
                .ThenBy(o => o.BirthOrder);
        }
    }
}
