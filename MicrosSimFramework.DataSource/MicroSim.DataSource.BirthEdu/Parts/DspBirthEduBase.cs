using MicroSim.DataSource.Core;
using MicroSim.DataSource.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.BirthEdu
{
    /// <summary>
    /// DspBirthEduBase class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSourcePart" />
    public class DspBirthEduBase : MsfDataSourcePart<BirthEduBaseEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DspBirthEduBase"/> class.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        public DspBirthEduBase(params MsfDataSourcePart[] inputs) 
            : base(inputs) { }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.DspBirthEduBase;

        /// <summary>
        /// Generates the data.
        /// </summary>
        protected override void GenerateData()
        {
            var birthNumbers = new List<BirthEduBaseEntity>();
            var birthNumbersRaw = GetInputDataOfType<BirthEduRawEntity>();
            
            foreach (var bnr in birthNumbersRaw)
            {
                var bn = new BirthEduBaseEntity();
                
                bn.Gender = Gender.Female;
                bn.Education = EducationExtensions.Parse(bnr.Education);
                var age = AgeExtensions.GetCorrectAge(bnr.Age);
                if (age == null ||
                    bn.Gender.IsFiltered() ||
                    bn.Education.IsFiltered())
                    continue;                
                bn.Age = (int)age;
                bn.Year = bnr.Year;
                bn.Value = bnr.Value;
                birthNumbers.Add(bn);
            }

            var ed02 = birthNumbers.Where(b => b.Education == Education.ED0_2);
            var nap = birthNumbers.Where(b => b.Education == Education.NAP);
            foreach (var bn in nap)
            {
                var other = ed02
                    .Where(b => 
                    b.Age == bn.Age &&
                    b.Year == bn.Year &&
                    b.Gender == bn.Gender)
                    .FirstOrDefault();
                if (other.Value == null)
                    other.Value = bn.Value;
                else
                    other.Value += bn.Value ?? 0;
            }

            Data = EducationExtensions.DistributeUnknowns<BirthEduBaseEntity>(birthNumbers.Except(nap));
        }
    }
}
