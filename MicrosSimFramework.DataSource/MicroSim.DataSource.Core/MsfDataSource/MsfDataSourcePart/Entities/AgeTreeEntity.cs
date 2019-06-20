using MicroSim.DataSource.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MicroSim.DataSource.Core
{
    /// <summary>
    /// Class used in creating Age Trees
    /// </summary>
    public class AgeTreeEntity : IPopulationEntity
    {
        public int Year { get; set; }
        public int Age { get; set; }
        public Gender Gender { get; set; }
        public decimal? Value { get; set; }
    }
}
