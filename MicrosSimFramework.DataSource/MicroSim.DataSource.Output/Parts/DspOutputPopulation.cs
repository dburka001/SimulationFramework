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
    /// DspOutputPopulation class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSourcePart" />
    public class DspOutputPopulation : MsfDataSourcePart<OutputPopulationEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DspOutputPopulation"/> class.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        public DspOutputPopulation(params MsfDataSourcePart[] inputs) 
            : base(inputs) { }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.DspOutputPopulation;

        /// <summary>
        /// Generates the data.
        /// </summary>
        protected override void GenerateData()
        {            
            var output = new List<OutputPopulationEntity>();
            var data = GetInputDataOfType<StartingEntity>()
                .Where(d => d.Age <= Settings.AgeLimit);

            foreach (var d in data)
            {
                output.Add(new OutputPopulationEntity()
                {
                    SocialGroup = (byte)d.SocialGroup,
                    ActiveYear = d.ActiveYear,
                    BirthYear = Settings.StartYear - d.Age + (d.ActiveYear - Settings.StartYear),
                    Education = (byte)d.Education,
                    Gender = (byte)d.Gender,
                    BirthOrder = d.BirthOrder == BirthOrder.Total 
                        ? (byte)0
                        : (byte)((int)d.BirthOrder - 2),                    
                    Weight = Math.Round((decimal)d.Value, 0)
                });
            }

            Data = output
                .Where(o => o.BirthYear != Settings.StartYear)
                .OrderBy(o => o.SocialGroup)
                .ThenBy(o => o.Gender)
                .ThenBy(o => o.BirthYear)
                .ThenBy(o => o.Education)
                .ThenBy(o => o.BirthOrder)
                .ThenBy(o => o.ActiveYear);
        }
    }
}
