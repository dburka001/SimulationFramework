using MicroSim.DataSource.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.Entities
{
    public class EduBasedVarEntity : IEducationEntity
    {
        public Education Education { get; set; }
        public decimal? StartYearOfWork { get; set; }
        public decimal? StartingIncome { get; set; }
        public decimal? ProbabilityOfEntrantWorkStart { get; set; }
    }
}
