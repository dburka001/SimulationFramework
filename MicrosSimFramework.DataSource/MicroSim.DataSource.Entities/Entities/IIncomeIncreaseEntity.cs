using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.Entities
{
    /// <summary>
    /// Interface for handling the income increase values
    /// </summary>
    public interface IIncomeIncreaseDisplayEntity : IEducationEntity, IValueEntity
    {
        int WorkYears { get; set; }
        decimal StartingIncome { get; set; }
        int StartYearOfWork { get; set; }
    }
}
