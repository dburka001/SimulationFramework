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
    /// DspOutputPensionBase class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSourcePart" />
    public class DspOutputPensionBase : MsfDataSourcePart<OutputPensionBaseEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DspOutputPensionBase"/> class.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        public DspOutputPensionBase(params MsfDataSourcePart[] inputs) 
            : base(inputs) { }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.DspOutputPensionBase;

        /// <summary>
        /// Generates the data.
        /// </summary>
        protected override void GenerateData()
        {            
            var output = new List<OutputPensionBaseEntity>();
            var data = GetInputDataOfType<PensionBasedVarEntity>();

            foreach (var d in data)
            {
                output.Add(new OutputPensionBaseEntity()
                {
                    PensionType = d.PensionType,
                    Value = d.BasePension
                });
            }

            Data = output
                .OrderBy(o => o.PensionType);
        }
    }
}
