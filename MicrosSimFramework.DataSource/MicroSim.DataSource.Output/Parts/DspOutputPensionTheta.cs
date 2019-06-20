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
    /// DspOutputPensionTheta class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSourcePart" />
    public class DspOutputPensionTheta : MsfDataSourcePart<OutputPensionThetaEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DspOutputPensionTheta"/> class.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        public DspOutputPensionTheta(params MsfDataSourcePart[] inputs) 
            : base(inputs) { }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.DspOutputPensionTheta;

        /// <summary>
        /// Generates the data.
        /// </summary>
        protected override void GenerateData()
        {            
            var output = new List<OutputPensionThetaEntity>();
            var data = GetInputDataOfType<PensionBasedVarEntity>();

            foreach (var d in data)
            {
                output.Add(new OutputPensionThetaEntity()
                {
                    PensionType = d.PensionType,
                    Value = d.Theta
                });
            }

            Data = output
                .OrderBy(o => o.PensionType);
        }
    }
}
