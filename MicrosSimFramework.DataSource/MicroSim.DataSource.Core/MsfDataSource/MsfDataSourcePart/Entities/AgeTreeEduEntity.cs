﻿using MicroSim.DataSource.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.Entities
{
    public class AgeTreeEduEntity : IPopulationEduEntity
    {        
        public Gender Gender { get; set; }
        public Education Education { get; set; }
        public int Age { get; set; }
        public int Year { get; set; }
        public decimal? Value { get; set; }
    }
}
