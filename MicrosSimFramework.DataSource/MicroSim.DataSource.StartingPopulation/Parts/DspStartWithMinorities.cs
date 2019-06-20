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
    /// DspStartWithMinorities class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSourcePart" />
    public class DspStartWithMinorities : MsfDataSourcePart<StartingEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DspStartWithMinorities"/> class.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        public DspStartWithMinorities(params MsfDataSourcePart[] inputs) 
            : base(inputs) { }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.DspStartWithMinorities;

        /// <summary>
        /// Generates the data.
        /// </summary>
        protected override void GenerateData()
        {
            var output = new List<StartingEntity>();
            var data = GetInputDataOfType<StartingEntity>();
            var minorities = GetInputDataOfType<MinorityRateEntity>();

            var minorityGroups = minorities
                .GroupBy(m => new { m.Age, m.Gender, m.Education })
                .Select(g => new {
                    g.Key.Age,
                    g.Key.Gender,
                    g.Key.Education,
                    Rates = g.Select(m => new { m.SocialGroup, m.Value }).ToList(),
                });

            var completeData = data
                .Join(minorityGroups,
                d => new { d.Age, d.Gender, d.Education },
                m => new { m.Age, m.Gender, m.Education },
                (d, m) => new {
                    d.Year,
                    d.Age,
                    d.Gender,
                    d.Education,
                    d.BirthOrder,
                    d.Value,
                    m.Rates
                });

            foreach (var d in completeData)
            {
                foreach (var r in d.Rates)
                {
                    output.Add(new StartingEntity()
                    {
                        SocialGroup = r.SocialGroup,
                        ActiveYear = d.Year,
                        Year = d.Year,
                        Age = d.Age,
                        Gender = d.Gender,
                        Education = d.Education,
                        BirthOrder = d.BirthOrder,                        
                        Value = d.Value * r.Value
                    });
                }
            }

            Data = output;
        }
    }
}
