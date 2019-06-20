using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.Entities
{
    /// <summary>
    /// IBirthCompleteEntity
    /// </summary>
    public interface IBirthCompleteRawEntity : IYearEntity, IAgeIntervalEntity, IValueEntity
    {
        string Education { get; set; }
        string NumberOfChildren { get; set; }     
    }
}
