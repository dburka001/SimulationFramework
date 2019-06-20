using MicroSim.DataSource.Core;
using MicroSim.DataSource.Entities;
using MicroSim.DataSource.DataAccess;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicroSim.DataSource.Rtools;

namespace MicroSim.DataSource.Population
{
    /// <summary>
    /// DspPopulationExt class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSourcePart" />
    public class DspPopulationExt : MsfDataSourcePart<PopulationBaseEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DspPopulationExt"/> class.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        public DspPopulationExt(params MsfDataSourcePart[] inputs) 
            : base(inputs) { }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.DspPopulationExt;

        /// <summary>
        /// Generates the data.
        /// </summary>
        protected override void GenerateData()
        {
            var population = new List<PopulationBaseEntity>();
            var populationBase = GetInputDataOfType<PopulationBaseEntity>();

            var maxAge = populationBase.Max(p => p.Age);           

            foreach (var pb in populationBase)
            {
                var p = new PopulationBaseEntity();
                p.Age = pb.Age;
                p.Gender = pb.Gender;
                p.Year = pb.Year;
                p.Value = pb.Value;

                if (p.Age != maxAge)
                {
                    population.Add(p);
                    continue;
                }
                
                var data = populationBase
                    .Where(d => d.Gender == p.Gender && d.Year == p.Year);
                var openData = data
                    .Where(d => d.Value == null);
                var openAgeStart = openData.Count() == 0 ? maxAge : openData.Min(d => d.Age);
                if (openAgeStart > Settings.AgeLimit)
                {
                    population.Add(p);
                    continue;
                }

                var lastValue = data
                    .Where(d => d.Age == openAgeStart - 1)                    
                    .Select(d => d.Value)
                    .FirstOrDefault();
                
                double[] newValues = 
                    Rscripts.ExtrapolatePopulation(
                        Convert.ToDouble(lastValue), 
                        Settings.AgeLimit - openAgeStart + 1, 
                        Convert.ToDouble(lastValue + p.Value));
                
                for (int a = openAgeStart; a <= Settings.AgeLimit; a++)
                {
                    PopulationBaseEntity pNew = population
                        .Where(pn =>
                        pn.Gender == pb.Gender &&
                        pn.Year == pb.Year &&
                        pn.Age == a)
                        .FirstOrDefault();
                    if (pNew == null)
                    {
                        pNew = new PopulationBaseEntity();
                        pNew.Age = a;
                        pNew.Gender = p.Gender;
                        pNew.Year = p.Year;
                        population.Add(pNew);
                    }
                    pNew.Value = Convert.ToDecimal(newValues[a - openAgeStart]);                    
                }
            }

            Data = population;
        }
    }
}
