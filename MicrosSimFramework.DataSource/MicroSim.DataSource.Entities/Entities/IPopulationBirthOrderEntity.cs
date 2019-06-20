using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.Entities
{
    /// <summary>
    /// Interface for handling basic population properties with birth order
    /// </summary>
    public interface IPopulationBirthOrderEntity : IPopulationEntity, IBirthOrderEntity
    { }
}
