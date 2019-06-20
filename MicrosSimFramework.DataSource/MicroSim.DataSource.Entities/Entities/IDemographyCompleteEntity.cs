using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.Entities
{
    /// <summary>
    /// Interface for handling education and birt order based demographic rates
    /// </summary>
    public interface IDemographyCompleteEntity : IDemographyEduEntity, IPopulationCompleteEntity
    { }
}
