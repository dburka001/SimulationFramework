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
    /// DspOutputFertilityTotal class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSourcePart" />
    public class DspOutputFertilityTotal : MsfDataSourcePart<OutputFertilityTotalEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DspOutputFertilityTotal"/> class.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        public DspOutputFertilityTotal(params MsfDataSourcePart[] inputs) 
            : base(inputs) { }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.DspOutputFertilityTotal;

        /// <summary>
        /// Generates the data.
        /// </summary>
        protected override void GenerateData()
        {            
            var output = new List<OutputFertilityTotalEntity>();
            var data = GetInputDataOfType<FertilityCompleteBaseEntity>()
                .Where(d => d.Education == Education.Total && d.BirthOrder == BirthOrder.Total);

            foreach (var d in data)
            {
                output.Add(new OutputFertilityTotalEntity()
                {
                    Age = d.Age,
                    Value = d.Value,
                    Year = d.Year
                });
            }

            Data = output
                .Where(o => o.Year >= Settings.StartYearOfValidation)
                .OrderBy(o => o.Year)
                .ThenBy(o => o.Age);
        }
    }
}
