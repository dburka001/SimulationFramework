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
    /// DspOutputMortality class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSourcePart" />
    public class DspOutputMortality : MsfDataSourcePart<OutputMortalityEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DspOutputMortality"/> class.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        public DspOutputMortality(params MsfDataSourcePart[] inputs) 
            : base(inputs) { }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.DspOutputMortality;

        /// <summary>
        /// Generates the data.
        /// </summary>
        protected override void GenerateData()
        {            
            var output = new List<OutputMortalityEntity>();
            var data = GetInputDataOfType<MortalityEduBaseEntity>();

            foreach (var d in data)
            {
                output.Add(new OutputMortalityEntity()
                {
                    Age = d.Age,
                    Education = d.Education,
                    Gender = d.Gender,
                    Value = d.Value,
                    Year = d.Year
                });
            }

            Data = output
                .Where(o => o.Year >= Settings.StartYearOfValidation)
                .OrderBy(o => o.Year)
                .OrderBy(o => o.Gender)
                .ThenBy(o => o.Age)
                .ThenBy(o => o.Education);
        }
    }
}
