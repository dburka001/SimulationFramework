using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.Entities
{
    /// <summary>
    /// Interface for handling basic population properties
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Entities.IValueEntity" />
    /// <seealso cref="MicroSim.DataSource.Entities.IYearEntity" />
    /// <seealso cref="MicroSim.DataSource.Entities.IAgeEntity" />
    /// <seealso cref="MicroSim.DataSource.Entities.IGenderEntity" />
    public interface IPopulationEntity : IYearEntity, IAgeEntity, IGenderEntity, IValueEntity
    { }
}
