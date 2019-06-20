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
    /// DspOutputPensionMultiplier class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSourcePart" />
    public class DspOutputPensionMultiplier : MsfDataSourcePart<OutputPensionMultiplierEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DspOutputWorkStatus"/> class.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        public DspOutputPensionMultiplier(params MsfDataSourcePart[] inputs) 
            : base(inputs) { }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.DspOutputPensionMultiplier;

        /// <summary>
        /// Generates the data.
        /// </summary>
        protected override void GenerateData()
        {            
            var output = new List<OutputPensionMultiplierEntity>();
            var data = GetInputDataOfType<PensionMultiplierEntity>();

            foreach (var d in data)
            {
                output.Add(new OutputPensionMultiplierEntity()
                {
                    WorkYears = d.WorkYears,
                    Value = d.Value
                });
            }

            Data = output
                .OrderBy(o => o.WorkYears);
        }
    }
}
