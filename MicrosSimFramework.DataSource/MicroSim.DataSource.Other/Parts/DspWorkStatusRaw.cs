﻿using MicroSim.DataSource.Core;
using MicroSim.DataSource.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.Other
{
    /// <summary>
    /// DspWorkStatusRaw class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSourcePart" />
    public class DspWorkStatusRaw : MsfDataSourcePart<WorkStatusRawEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DspWorkStatusRaw"/> class.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        public DspWorkStatusRaw(string inputPath) 
            : base(inputPath) { }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.DspWorkStatusRaw;
    }
}
