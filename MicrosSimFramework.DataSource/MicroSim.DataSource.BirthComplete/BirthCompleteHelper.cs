using MicroSim.DataSource.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.BirthComplete
{
    /// <summary>
    /// BirthCompleteHelper
    /// </summary>
    public static class BirthCompleteHelper<T> where T: IPopulationCompleteEntity
    {
        public static IEnumerable<T> DistributeValues(
            IEnumerable<IBirthCompleteRawEntity> rawData,
            IEnumerable<IPopulationEduEntity> originalData)
        {
            var newData = new List<T>();
            var originalAgeStart = originalData.Min(o => o.Age);
            var originalAgeEnd = originalData.Max(o => o.Age);

            foreach (var o in rawData)
            {
                var year = o.Year;
                var interval = AgeIntervalExtensions.GetAgeInterval(o.AgeInterval);
                var ageStart = interval.Item1;
                var ageEnd = interval.Item2;
                var gender = Gender.Female;
                var education = EducationExtensions.Parse(o.Education);
                var birthOrder = BirthOrderExtensions.Parse(o.NumberOfChildren);
                if (gender.IsFiltered() ||
                    education.IsFiltered() ||
                    birthOrder.IsFiltered())
                    continue;

                var currentOriginal = originalData
                        .Where(p =>
                        p.Age >= ageStart &&
                        p.Age <= ageEnd &&
                        p.Gender == gender &&
                        p.Education == education);
                
                var yearMin = currentOriginal
                    .Where(b => b.Value != null)
                    .Min(b => b.Year);

                var data = currentOriginal
                    .Where(d => d.Year == Math.Max(year, yearMin));

                var currentSum = data.Sum(d => d.Value);

                for (int a = ageStart; a <= ageEnd; a++)
                {
                    var currentValue = data
                        .Where(d => d.Age == a)
                        .Sum(d => d.Value);

                    var n = (T)Activator.CreateInstance(typeof(T));
                    n.Age = a;
                    n.BirthOrder = birthOrder;
                    n.Education = education;
                    n.Year = year;
                    n.Gender = gender;
                    n.Value = o.Value * currentValue / currentSum;

                    var checkNew = newData
                        .Where(nd =>
                        nd.Age == n.Age &&
                        nd.BirthOrder == n.BirthOrder &&
                        nd.Education == n.Education &&
                        nd.Year == n.Year &&
                        nd.Gender == n.Gender)
                        .FirstOrDefault();

                    if (checkNew == null)
                        newData.Add(n);
                    else
                        checkNew.Value += n.Value;
                }
            }

            return newData;
        }
    }
}
