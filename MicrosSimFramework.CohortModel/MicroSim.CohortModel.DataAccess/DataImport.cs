using MicroSim.CohortModel.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.CohortModel.DataAccess
{
    public static class DataImport
    {
        private static string WorkingDirectory = @"c:\Users\dburk\Dropbox\Tervezet\SimulationFramework\Adatok\";

        public static List<T> LoadCSV<T>(string fileName, int startYear = 0)
        {
            var list = new List<T>();

            using (StreamReader sr = new StreamReader(Path.Combine(WorkingDirectory, fileName), Encoding.Default))
            {
                sr.ReadLine(); // Header
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    object p = null;
                    if (typeof(T) == typeof(PopulationEntity))
                    {
                        p = new PopulationEntity()
                        {
                            Year = startYear,
                            Gender = Convert.ToInt32(line[0]),
                            BirthYear = Convert.ToInt32(line[1]),
                            Population = Convert.ToDecimal(line[2]),
                        };
                        
                    }
                    else if (typeof(T) == typeof(MortalityEntity))
                    {
                        if (line[3] != "Total") continue;
                        p = new MortalityEntity()
                        {
                            Year = Convert.ToInt32(line[0]),
                            Age = Convert.ToInt32(line[1]),
                            Gender = line[2] == "Male" ? 1 : 2,
                            Value = Convert.ToDecimal(line[4]),
                        };

                    }
                    else if (typeof(T) == typeof(FertilityEntity))
                    {
                        p = new FertilityEntity()
                        {
                            Year = Convert.ToInt32(line[0]),
                            Age = Convert.ToInt32(line[1]),                            
                            Value = Convert.ToDecimal(line[2]),
                        };

                    }

                    list.Add((T)Convert.ChangeType(p, typeof(T)));
                }
            }

            return list;
        }
    }
}
