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
    /// DspStartComplete class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSourcePart" />
    public class DspStartComplete : MsfDataSourcePart<StartingEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DspStartComplete"/> class.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        public DspStartComplete(params MsfDataSourcePart[] inputs) 
            : base(inputs) { }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.DspStartComplete;

        /// <summary>
        /// Generates the data.
        /// </summary>
        protected override void GenerateData()
        {
            var outputData = new List<StartingEntity>();
            var data = GetInputDataOfType<StartingEntity>();
            var mothers = GetInputDataOfType<BirthMotherEntity>()
                .Where(m => m.Education != Education.Total && m.BirthOrder != BirthOrder.Total);

            var motherYears = mothers.Select(m => m.Year).Distinct();
            var closestYear = motherYears.Min();
            foreach (var y in motherYears)
            {
                if (Math.Abs(Settings.StartYear - closestYear) > Math.Abs(Settings.StartYear - y))
                    closestYear = y;
            }

            foreach (var d in data)
            {
                outputData.Add(new StartingEntity()
                {
                    SocialGroup = SocialGroup.Majority,
                    ActiveYear = d.Year,
                    Age = d.Age,
                    Education = d.Education,
                    BirthOrder = d.BirthOrder,
                    Gender = d.Gender,
                    Year = d.Year,
                    Value = d.Value
                });
            }

            var motherData = outputData
                .Where(d =>
                d.Gender == Gender.Female &&
                d.Age >= mothers.Min(m => m.Age) &&
                d.Age <= mothers.Max(m => m.Age));

            var detailedData = motherData
                .Join(mothers.Where(m => m.Year == closestYear),
                d => new { d.Age, d.Gender, d.Education },
                m => new { m.Age, m.Gender, m.Education },
                (d, m) => new StartingEntity()
                {
                    SocialGroup = SocialGroup.Majority,
                    ActiveYear = d.Year,
                    Age = d.Age,
                    Education = d.Education,
                    BirthOrder = m.BirthOrder,
                    Gender = d.Gender,
                    Year = d.Year,
                    Value = m.Value
                });

            var output = outputData.Except(motherData).ToList();
            output.AddRange(detailedData);

            Data = output;
        }
    }
}
