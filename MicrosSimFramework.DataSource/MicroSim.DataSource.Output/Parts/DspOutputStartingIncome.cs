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
    /// DspOutputStartingIncome class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSourcePart" />
    public class DspOutputStartingIncome : MsfDataSourcePart<OutputStartingIncomeEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DspOutputStartingIncome"/> class.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        public DspOutputStartingIncome(params MsfDataSourcePart[] inputs) 
            : base(inputs) { }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.DspOutputStartingIncome;

        /// <summary>
        /// Generates the data.
        /// </summary>
        protected override void GenerateData()
        {            
            var output = new List<OutputStartingIncomeEntity>();
            var data = GetInputDataOfType<EduBasedVarEntity>();

            foreach (var d in data)
            {
                output.Add(new OutputStartingIncomeEntity()
                {
                    Education = d.Education,
                    Value = d.StartingIncome
                });
            }

            Data = output;
        }
    }
}
