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
    /// DspOutputWorkYearStart class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSourcePart" />
    public class DspOutputWorkYearStart : MsfDataSourcePart<OutputWorkYearStartEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DspOutputWorkYearStart"/> class.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        public DspOutputWorkYearStart(params MsfDataSourcePart[] inputs) 
            : base(inputs) { }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.DspOutputWorkYearStart;

        /// <summary>
        /// Generates the data.
        /// </summary>
        protected override void GenerateData()
        {            
            var output = new List<OutputWorkYearStartEntity>();
            var data = GetInputDataOfType<EduBasedVarEntity>();

            foreach (var d in data)
            {
                output.Add(new OutputWorkYearStartEntity()
                {
                    Education = d.Education,
                    Value = d.StartYearOfWork
                });
            }

            Data = output;
        }
    }
}
