using MicroSim.DataSource.Core;
using MicroSim.DataSource.Entities;
using MicroSim.DataSource.Rtools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.DeathEdu
{
    /// <summary>
    /// DspDeathEduExt class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSourcePart" />
    public class DspDeathEduExt : MsfDataSourcePart<DeathEduBaseEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DspDeathEduExt"/> class.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        public DspDeathEduExt(params MsfDataSourcePart[] inputs) 
            : base(inputs) { }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.DspDeathEduExt;

        /// <summary>
        /// Generates the data.
        /// </summary>
        protected override void GenerateData()
        {
            var deathNumbers = new List<DeathEduBaseEntity>();
            var deathNumbersBase = GetInputDataOfType<DeathEduBaseEntity>();
            
            var maxAge = deathNumbersBase.Max(d => d.Age);

            foreach (var d in deathNumbersBase)
            {                
                var dat = new DeathEduBaseEntity()
                {
                    Age = d.Age,
                    Gender = d.Gender,
                    Education = d.Education,
                    Year = d.Year,
                    Value = d.Value
                };
                deathNumbers.Add(dat);

                if (d.Age < maxAge || d.Age >= Settings.AgeLimit) continue;

                var lastValue = deathNumbersBase
                    .Where(l => 
                    l.Age == d.Age - 1 &&
                    l.Education == d.Education &&
                    l.Gender == d.Gender &&
                    l.Year == d.Year)
                    .Select(l => l.Value)
                    .FirstOrDefault();

                double[] newValues =
                    Rscripts.ExtrapolatePopulation(
                        Convert.ToDouble(lastValue),
                        Settings.AgeLimit - d.Age + 2,
                        Convert.ToDouble(lastValue + d.Value));

                dat.Value = Convert.ToDecimal(newValues[1]);

                for (int i = 2; i < newValues.Length; i++)
                {
                    deathNumbers.Add(new DeathEduBaseEntity()
                    {
                        Age = d.Age + i - 1,
                        Education = d.Education,
                        Gender = d.Gender,
                        Year = d.Year,
                        Value = Convert.ToDecimal(newValues[i])
                    });
                }
            }

            Data = deathNumbers;
        }
    }
}
