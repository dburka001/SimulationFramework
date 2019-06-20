using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.Entities
{
    /// <summary>
    /// Interface for handling basic population properties etended with education
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Entities.IPopulationEntity" />
    /// <seealso cref="MicroSim.DataSource.Entities.IEducationEntity" />
    public interface IPopulationEduEntity : IPopulationEntity, IEducationEntity
    { }
}
