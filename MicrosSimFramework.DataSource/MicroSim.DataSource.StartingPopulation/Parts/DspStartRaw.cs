using MicroSim.DataSource.Core;
using MicroSim.DataSource.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.StartingPopulation
{
    /// <summary>
    /// DspStartRaw class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSourcePart" />
    public class DspStartRaw : MsfDataSourcePart<StartingEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DspStartRaw"/> class.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        public DspStartRaw(params MsfDataSourcePart[] inputs) 
            : base(inputs) { }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.DspStartRaw;

        /// <summary>
        /// Generates the data.
        /// </summary>
        protected override void GenerateData()
        {            
            var output = new List<StartingEntity>();
            var data = GetInputDataOfType<PopulationEduEntity>();

            foreach (var d in data.Where(d => d.Year == Settings.StartYear))
            {
                output.Add(new StartingEntity()
                {
                    SocialGroup = SocialGroup.Majority,
                    ActiveYear = d.Year,
                    Age = d.Age,
                    Education = d.Education,
                    BirthOrder = BirthOrder.Total,
                    Gender = d.Gender,
                    Value = d.Value,
                    Year = d.Year
                });
            }

            Data = output;
        }
    }
}
