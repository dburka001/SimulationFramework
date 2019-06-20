using MicroSim.DataSource.Core;
using MicroSim.DataSource.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.DeathEdu
{
    /// <summary>
    /// DspDeathEduBase class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSourcePart" />
    public class DspDeathEduBase : MsfDataSourcePart<DeathEduBaseEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DspDeathEduBase"/> class.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        public DspDeathEduBase(params MsfDataSourcePart[] inputs) 
            : base(inputs) { }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.DspDeathEduBase;

        /// <summary>
        /// Generates the data.
        /// </summary>
        protected override void GenerateData()
        {
            var deathNumbers = new List<DeathEduBaseEntity>();
            var deathNumbersRaw = GetInputDataOfType<DeathEduRawEntity>();
            
            foreach (var dnr in deathNumbersRaw)
            {
                var dn = new DeathEduBaseEntity();
                
                dn.Gender = GenderExtensions.Parse(dnr.Sex);
                dn.Education = EducationExtensions.Parse(dnr.Education);
                var age = AgeExtensions.GetCorrectAge(dnr.Age);
                if (age == null ||
                    dn.Gender.IsFiltered() ||
                    dn.Education.IsFiltered())
                    continue;                
                dn.Age = (int)age;
                dn.Year = dnr.Year;
                dn.Value = dnr.Value;
                deathNumbers.Add(dn);
            }

            var ed02 = deathNumbers.Where(b => b.Education == Education.ED0_2);
            var nap = deathNumbers.Where(b => b.Education == Education.NAP);
            foreach (var dn in nap)
            {
                var other = ed02
                    .Where(b =>
                    b.Age == dn.Age &&
                    b.Year == dn.Year &&
                    b.Gender == dn.Gender)
                    .FirstOrDefault();
                if (other.Value == null)
                    other.Value = dn.Value;
                else
                    other.Value += dn.Value ?? 0;
            }

            var deaths = EducationExtensions.DistributeUnknowns<DeathEduBaseEntity>(deathNumbers.Except(nap));

            MsfExtensions.DistributeDetEducationValues(deaths);

            Data = deaths;
        }
    }
}
