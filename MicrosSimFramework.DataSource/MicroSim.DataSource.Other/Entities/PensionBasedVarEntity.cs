using MicroSim.DataSource.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.Entities
{
    public class PensionBasedVarEntity : IPensionType
    {
        public PensionType PensionType { get; set; }
        public decimal? BasePension { get; set; }
        public decimal? Theta { get; set; }
    }
}
