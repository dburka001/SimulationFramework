using MicroSim.CohortModel.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.CohortModel.Entities
{
    public sealed class CohortData
    {
        private const int _startYear = 2007;
        private const int _endYear = 2017;

        public List<PopulationEntity> Population { get; } 
            = DataImport.LoadCSV<PopulationEntity>("Validáló induló népesség.csv", _startYear);
        public List<MortalityEntity> Mortalities { get; }
            = DataImport.LoadCSV<MortalityEntity>("Mortalitás.csv");
        public List<FertilityEntity> Fertilities { get; }
            = DataImport.LoadCSV<FertilityEntity>("Fertilitás teljes.csv");

        /// <summary>
        /// The instance
        /// </summary>
        private static CohortData _instance;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static CohortData Instance
            => _instance ?? (_instance = new CohortData());

        /// <summary>
        /// Prevents a default instance of the <see cref="CohortData"/> class from being created.
        /// </summary>
        private CohortData()
        {
            ForeCast();
        }

        private void ForeCast()
        {                       
            for (int y = _startYear; y <= _endYear; y++)
            {
                var newPopulation = new List<PopulationEntity>();
                var currentPopulation = Population.Where(p => p.Year == y);
                
                // Birth
                foreach (var p in currentPopulation)
                {
                    var age = y - p.BirthYear;

                    // Birth
                    if (p.Gender == 2)
                    {
                        var fertility = Fertilities
                            .Where(f =>
                                f.Year == y &&
                                f.Age == age
                            );
                        var fert = fertility.Count() > 0 ? fertility.FirstOrDefault().Value : 0;
                        p.Births = Math.Round(p.Population * fert);
                    }
                }

                // NewBorns
                var newBorns = currentPopulation.Sum(p => p.Births);
                var maleProbability = 0.514m;
                for (int g = 1; g <= 2; g++)
                {
                    Population.Add(new PopulationEntity()
                    {
                        Year = y,
                        BirthYear = y,
                        Gender = g,
                        Deaths = 0,
                        Births = 0,
                        Population = Math.Round(newBorns * (g == 1 ? maleProbability : 1 - maleProbability))
                    });
                }

                // SimStep
                foreach (var p in currentPopulation)
                {
                    var age = y - p.BirthYear;

                    // Death
                    var mortality = Mortalities
                        .Where(m =>
                            m.Year == y &&
                            m.Age == age &&
                            m.Gender == p.Gender
                        );
                    var mort = mortality.Count() > 0 ? mortality.FirstOrDefault().Value : 1;
                    if(age != 0)
                        p.Deaths = Math.Round(p.Population * mort);

                    // New year
                    newPopulation.Add(new PopulationEntity()
                    {
                        Year = y + 1,
                        BirthYear = p.BirthYear,
                        Gender = p.Gender,
                        Deaths = 0,
                        Births = 0,
                        Population = p.Population - p.Deaths
                    });
                }

                Population.AddRange(newPopulation);                            
            }
        }
    }
}
