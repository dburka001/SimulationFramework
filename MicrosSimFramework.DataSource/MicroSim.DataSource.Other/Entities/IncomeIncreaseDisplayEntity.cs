using MicroSim.DataSource.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.Entities
{
    public class IncomeIncreaseDisplayEntity : IIncomeIncreaseDisplayEntity
    {
        public int WorkYears { get; set; }
        public Education Education { get; set; }
        public decimal? Value { get; set; }
        public decimal StartingIncome { get; set; }
        public int StartYearOfWork { get; set; }
    }
}
