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
    /// DspOutputEconomicGrowth class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSourcePart" />
    public class DspOutputEconomicGrowth : MsfDataSourcePart<OutputEconomicGrowthEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DspOutputEconomicGrowth"/> class.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        public DspOutputEconomicGrowth(params MsfDataSourcePart[] inputs) 
            : base(inputs) { }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.DspOutputEconomicGrowth;

        /// <summary>
        /// Generates the data.
        /// </summary>
        protected override void GenerateData()
        {            
            var output = new List<OutputEconomicGrowthEntity>();
            var data = GetInputDataOfType<EconomicGrowthEntity>();

            foreach (var d in data)
            {
                output.Add(new OutputEconomicGrowthEntity()
                {
                    Year = d.Year,
                    Value = d.Value
                });
            }

            Data = output
                .OrderBy(o => o.Year);
        }
    }
}
