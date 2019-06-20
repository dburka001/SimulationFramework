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
    /// DspOutputWorkStatusMaxTime class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSourcePart" />
    public class DspOutputWorkStatusMaxTime : MsfDataSourcePart<OutputWorkStatusMaxTimeEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DspOutputWorkStatusMaxTime"/> class.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        public DspOutputWorkStatusMaxTime(params MsfDataSourcePart[] inputs) 
            : base(inputs) { }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.DspOutputWorkStatusMaxTime;

        /// <summary>
        /// Generates the data.
        /// </summary>
        protected override void GenerateData()
        {            
            var output = new List<OutputWorkStatusMaxTimeEntity>();
            var data = GetInputDataOfType<WorkStatusTimeEntity>();

            foreach (var d in data)
            {
                output.Add(new OutputWorkStatusMaxTimeEntity()
                {
                    WorkStatus = d.WorkStatus,
                    Value = d.MaxTime
                });
            }

            Data = output
                .OrderBy(o => o.WorkStatus);
        }
    }
}
