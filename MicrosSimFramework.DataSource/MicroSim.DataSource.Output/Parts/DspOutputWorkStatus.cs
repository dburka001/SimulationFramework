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
    /// DspOutputWorkStatus class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSourcePart" />
    public class DspOutputWorkStatus : MsfDataSourcePart<OutputWorkStatusEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DspOutputWorkStatus"/> class.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        public DspOutputWorkStatus(params MsfDataSourcePart[] inputs) 
            : base(inputs) { }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.DspOutputWorkStatus;

        /// <summary>
        /// Generates the data.
        /// </summary>
        protected override void GenerateData()
        {            
            var output = new List<OutputWorkStatusEntity>();
            var data = GetInputDataOfType<WorkStatusEntity>()
                .OrderBy(d => d.Age)
                .ThenBy(d => d.Education)
                .ThenBy(d => d.WorkStatusCurrent)
                .ThenBy(d => d.WorkStatusNext);

            WorkStatus? last = null;
            decimal? lastValue = 0;

            foreach (var d in data)
            {
                if (last != d.WorkStatusCurrent)
                    lastValue = 0;

                lastValue += d.Value;
                last = d.WorkStatusCurrent;

                output.Add(new OutputWorkStatusEntity()
                {
                    Age = d.Age,
                    Education = d.Education,
                    WorkStatusCurrent = d.WorkStatusCurrent,
                    WorkStatusNext = d.WorkStatusNext,
                    Value = lastValue
                });                
            }

            Data = output;
        }
    }
}
