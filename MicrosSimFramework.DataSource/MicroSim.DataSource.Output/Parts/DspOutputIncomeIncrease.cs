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
    /// DspOutputIncomeIncrease class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSourcePart" />
    public class DspOutputIncomeIncrease : MsfDataSourcePart<OutputIncomeIncreaseEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DspOutputIncomeIncrease"/> class.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        public DspOutputIncomeIncrease(params MsfDataSourcePart[] inputs) 
            : base(inputs) { }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.DspOutputIncomeIncrease;

        /// <summary>
        /// Generates the data.
        /// </summary>
        protected override void GenerateData()
        {            
            var output = new List<OutputIncomeIncreaseEntity>();
            var data = GetInputDataOfType<IncomeIncreaseEntity>();

            foreach (var d in data)
            {
                output.Add(new OutputIncomeIncreaseEntity()
                {
                    WorkYears = d.WorkYears,
                    Education = d.Education,
                    Value = d.Value
                });
            }

            Data = output
                .OrderBy(o => o.WorkYears)
                .ThenBy(o => o.Education);
        }
    }
}
