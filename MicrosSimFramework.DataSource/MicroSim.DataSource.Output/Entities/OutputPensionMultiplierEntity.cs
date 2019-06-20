using MicroSim.DataSource.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.Entities
{
    public class OutputPensionMultiplierEntity : IValueEntity
    {
        [MsfDisplayName(nameof(WorkYears))]
        public decimal? WorkYears { get; set; }
        public decimal? Value { get; set; }
    }
}
