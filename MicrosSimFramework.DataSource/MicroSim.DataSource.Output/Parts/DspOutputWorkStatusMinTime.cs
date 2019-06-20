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
    /// DspOutputWorkStatusMinTime class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSourcePart" />
    public class DspOutputWorkStatusMinTime : MsfDataSourcePart<OutputWorkStatusMinTimeEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DspOutputWorkStatusMinTime"/> class.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        public DspOutputWorkStatusMinTime(params MsfDataSourcePart[] inputs) 
            : base(inputs) { }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.DspOutputWorkStatusMinTime;

        /// <summary>
        /// Generates the data.
        /// </summary>
        protected override void GenerateData()
        {            
            var output = new List<OutputWorkStatusMinTimeEntity>();
            var data = GetInputDataOfType<WorkStatusTimeEntity>();

            foreach (var d in data)
            {
                output.Add(new OutputWorkStatusMinTimeEntity()
                {
                    WorkStatus = d.WorkStatus,
                    Value = d.MinTime
                });
            }

            Data = output
                .OrderBy(o => o.WorkStatus);
        }
    }
}
