﻿using MicroSim.DataSource.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.Entities
{
    public class OutputWorkStatusEntity : IAgeEntity, IEducationEntity, IValueEntity
    {                
        public Education Education { get; set; }
        public int Age { get; set; }
        [MsfDisplayName(nameof(WorkStatusCurrent))]
        public WorkStatus WorkStatusCurrent { get; set; }
        [MsfDisplayName(nameof(WorkStatusNext))]
        public WorkStatus WorkStatusNext { get; set; }
        public decimal? Value { get; set; }        
    }
}
