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
    /// DspOutputPopulationValidation class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSourcePart" />
    public class DspOutputPopulationValidation : MsfDataSourcePart<OutputPopulationValidationEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DspOutputPopulationValidation"/> class.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        public DspOutputPopulationValidation(params MsfDataSourcePart[] inputs) 
            : base(inputs) { }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.DspOutputPopulationValidation;

        /// <summary>
        /// Generates the data.
        /// </summary>
        protected override void GenerateData()
        {            
            var output = new List<OutputPopulationValidationEntity>();
            var data = GetInputDataOfType<StartingValidationEntity>()
                .Where(d => d.Age <= Settings.AgeLimit);

            foreach (var d in data)
            {
                output.Add(new OutputPopulationValidationEntity()
                {
                    BirthYear = Settings.StartYearOfValidation - d.Age,
                    Gender = (byte)d.Gender,                 
                    Weight = Math.Round((decimal)d.Value, 0)
                });
            }

            Data = output
                .Where(o => o.BirthYear != Settings.StartYearOfValidation)
                .OrderBy(o => o.Gender)
                .ThenBy(o => o.BirthYear);
        }
    }
}
