using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.Entities
{
    public class BirthMotherRawEntity : IBirthCompleteRawEntity
    {
        public int Year { get; set; }
        public string AgeInterval { get; set; }
        public string Education { get; set; }
        public string NumberOfChildren { get; set; }        
        public decimal? Value { get; set; }
        
    }
}
