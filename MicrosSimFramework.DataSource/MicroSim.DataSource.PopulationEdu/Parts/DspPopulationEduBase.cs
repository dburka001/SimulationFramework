using MicroSim.DataSource.Core;
using MicroSim.DataSource.Entities;
using MicroSim.DataSource.DataAccess;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.PopulationEdu
{
    /// <summary>
    /// DspPopulationEduBase class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSourcePart" />
    public class DspPopulationEduBase : MsfDataSourcePart<PopulationEduBaseEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DspPopulationEduBase"/> class.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        public DspPopulationEduBase(params MsfDataSourcePart[] inputs)
            : base(inputs) { }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.DspPopulationEduBase;

        /// <summary>
        /// Generates the data.
        /// </summary>
        protected override void GenerateData()
        {
            var population = new List<PopulationEduBaseEntity>();
            var populationRaw = GetInputDataOfType<PopulationEduRawEntity>();

            var acceptedIntervals = new string[]
            {
                "Y15-19",
                "Y20-24",
                "Y25-34",
                "Y35-44",
                "Y45-54",
                "Y55-64",
                "Y55-74",
            };

            foreach (var pr in populationRaw)
            {
                var pb = new PopulationEduBaseEntity();
                pb.Year = pr.Year;
                pb.Gender = GenderExtensions.Parse(pr.Sex);
                pb.Education = EducationExtensions.Parse(pr.Education);
                if (pb.Gender.IsFiltered() ||
                    pb.Education.IsFiltered() ||
                    !acceptedIntervals.Contains(pr.AgeInterval))
                    continue;
                if (pr.AgeInterval == "Y55-74")
                {
                    pb.AgeStart = 65;
                    pb.AgeEnd = 74;
                    var otherValue = populationRaw
                        .Where(p =>
                        p.Education == pr.Education && 
                        p.Sex == pr.Sex &&                        
                        p.AgeInterval == "Y55-64" &&
                        p.Year == pr.Year)
                        .FirstOrDefault().Value;
                    pb.Value = pr.Value - otherValue;
                }
                else
                {
                    var interval = AgeIntervalExtensions.GetAgeInterval(pr.AgeInterval);
                    pb.AgeStart = interval.Item1;
                    pb.AgeEnd = interval.Item2;
                    pb.Value = pr.Value;
                }
                pb.Value = pb.Value * 1000;
                population.Add(pb);
            }
            
            Data = population;
        }
    }
}
