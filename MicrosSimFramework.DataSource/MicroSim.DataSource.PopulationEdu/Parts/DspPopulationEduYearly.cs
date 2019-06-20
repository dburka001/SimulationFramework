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
    /// DspPopulationEduYearly class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSourcePart" />
    public class DspPopulationEduYearly : MsfDataSourcePart<PopulationEduEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DspPopulationEduYearly"/> class.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        public DspPopulationEduYearly(params MsfDataSourcePart[] inputs)
           : base(inputs) { }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.DspPopulationEduYearly;

        /// <summary>
        /// Generates the data.
        /// </summary>
        protected override void GenerateData()
        {
            var population = new List<PopulationEduEntity>();
            var populationBase = GetInputDataOfType<PopulationBaseEntity>();
            var populationEdu = GetInputDataOfType<PopulationEduBaseEntity>();

            var eduYears = populationEdu.Select(e => e.Year).Distinct();

            foreach (var pb in populationBase.Where(p => eduYears.Contains(p.Year)))
            {
                var currentEduData = GetClosestEduData(pb, populationEdu);
                decimal? currentSum = currentEduData.Count() != 0 
                    ? currentEduData.Sum(c => c.Value) 
                    : null;
                foreach (var eduLevel in Enum.GetValues(typeof(Education)).Cast<Education>())
                {
                    if (eduLevel.IsFiltered()) continue;

                    var p = new PopulationEduEntity()
                    {
                        Age = pb.Age,
                        Education = eduLevel,
                        Gender = pb.Gender,
                        Year = pb.Year
                    };

                    PopulationEduBaseEntity current = currentEduData
                        .Where(pe => pe.Education == eduLevel)
                        .FirstOrDefault();

                    if (current != null && currentSum != 0)
                        p.Value = pb.Value * current.Value / currentSum;

                    population.Add(p);
                }                
            }

            var nap = population.Where(p => p.Education == Education.NAP);

            Data = EducationExtensions.DistributeUnknowns<PopulationEduEntity>(population.Except(nap));
        }

        /// <summary>
        /// Gets the closest edu data.
        /// </summary>
        /// <param name="pb">The pb.</param>
        /// <param name="populationEdu">The population edu.</param>
        /// <returns></returns>
        private IEnumerable<PopulationEduBaseEntity> GetClosestEduData(
            PopulationBaseEntity pb,
            IEnumerable<PopulationEduBaseEntity> populationEdu)
        {
            var data = populationEdu
                    .Where(pe =>
                    pe.Gender == pb.Gender &&
                    pe.Year == pb.Year &&
                    pe.Value != null);

            if (data.Count() == 0) return data;
            var minYear = Math.Max(data.Min(d => d.AgeStart), Settings.DetEducationAgeStart);
            var maxYear = data.Max(d => d.AgeEnd);

            if (pb.Age > maxYear)
                return data
                    .Where(pe =>
                    pe.AgeEnd == maxYear);
            else if (pb.Age < minYear)
                return data
                    .Where(pe =>
                    pe.AgeStart <= minYear &&
                    pe.AgeEnd >= minYear);
            else
                return data
                        .Where(pe =>
                        pe.AgeStart <= pb.Age &&
                        pe.AgeEnd >= pb.Age);
        }
    }
}
