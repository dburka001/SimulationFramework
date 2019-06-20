using MicroSim.DataSource.Core;
using MicroSim.DataSource.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.StartingPopulation
{
    /// <summary>
    /// DspStartValidation class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSourcePart" />
    public class DspStartValidation : MsfDataSourcePart<StartingValidationEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DspStartValidation"/> class.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        public DspStartValidation(params MsfDataSourcePart[] inputs) 
            : base(inputs) { }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.DspStartValidation;

        /// <summary>
        /// Generates the data.
        /// </summary>
        protected override void GenerateData()
        {            
            var output = new List<StartingValidationEntity>();
            var data = GetInputDataOfType<PopulationBaseEntity>();

            foreach (var d in data.Where(d => d.Year == Settings.StartYearOfValidation))
            {
                output.Add(new StartingValidationEntity()
                {
                    Age = d.Age,
                    Gender = d.Gender,
                    Value = d.Value,
                    Year = d.Year
                });
            }

            Data = output;
        }
    }
}
