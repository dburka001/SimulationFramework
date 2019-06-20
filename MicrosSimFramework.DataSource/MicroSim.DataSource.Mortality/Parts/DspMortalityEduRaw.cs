using MicroSim.DataSource.Core;
using MicroSim.DataSource.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.Mortality
{
    /// <summary>
    /// DspMortalityEduRaw class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSourcePart" />
    public class DspMortalityEduRaw : MsfDataSourcePart<MortalityEduBaseEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DspMortalityEduRaw"/> class.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        public DspMortalityEduRaw(params MsfDataSourcePart[] inputs)
            : base(inputs) { }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.DspMortalityEduRaw;

        /// <summary>
        /// Generates the data.
        /// </summary>
        protected override void GenerateData()
        {
            var population = GetInputDataOfType<PopulationBaseEntity>();
            var death = GetInputDataOfType<DeathBaseEntity>();
            var birth = GetInputDataOfType<BirthBaseEntity>();

            var totalBirth = birth
                .GroupBy(b => new { b.Year })
                .Select(g => new
                {
                    g.Key.Year,
                    BirthCount = g.Sum(b => b.Value)
                });

            var mortality = population
                .Join(death,
                (p) => new { p.Age, p.Gender, p.Year },
                (d) => new { d.Age, d.Gender, d.Year },
                (p, d) => new { p, d })
                .Join(totalBirth,
                (p) => new { p.p.Year },
                (b) => new { b.Year },
                (p, b) => new MortalityEduBaseEntity()
                {
                    Age = p.p.Age,
                    Education = Education.Total,
                    Gender = p.p.Gender,
                    Year = p.p.Year,
                    Value = MsfExtensions.GetProbabilityValue(p.p.Value, p.d.Value),
                    Population = p.p.Value,
                    DeathCount = p.d.Value,
                    BirthCount = b.BirthCount
                })
                .Where(m => m.Age <= Settings.AgeLimit)
                .Where(m => m.Year <= Settings.StartYear)
                .ToList();

            var populationEdu = GetInputDataOfType<PopulationEduEntity>();
            var deathEdu = GetInputDataOfType<DeathEduBaseEntity>();
            var birthEdu = GetInputDataOfType<BirthEduBaseEntity>();

            var totalBirthEdu = birthEdu
                .GroupBy(b => new { b.Year, b.Education })
                .Select(g => new
                {
                    g.Key.Year,
                    g.Key.Education,
                    BirthCount = g.Sum(b => b.Value)
                });

            var mortalityEdu = populationEdu                
                .Join(deathEdu,
                (p) => new { p.Age, p.Education, p.Gender, p.Year },
                (d) => new { d.Age, d.Education, d.Gender, d.Year },
                (p, d) => new { p, d })
                .Join(totalBirthEdu,
                (p) => new { p.p.Year, p.p.Education },
                (b) => new { b.Year, b.Education },
                (p, b) => new MortalityEduBaseEntity()
                {
                    Age = p.p.Age,
                    Education = p.p.Education,
                    Gender = p.p.Gender,
                    Year = p.p.Year,
                    Value = MsfExtensions.GetProbabilityValue(p.p.Value, p.d.Value),
                    Population = p.p.Value,
                    DeathCount = p.d.Value,
                    BirthCount = b.BirthCount
                })
                .Where(m => m.Age <= Settings.AgeLimit)
                .Where(m => m.Year <= Settings.StartYear);


            mortality.AddRange(mortalityEdu);

            Data = mortality;
        }
    }
}
