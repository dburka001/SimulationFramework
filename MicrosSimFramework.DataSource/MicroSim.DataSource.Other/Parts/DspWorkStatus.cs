using MicroSim.DataSource.Core;
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
    /// DspWorkStatus class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSourcePart" />
    public class DspWorkStatus : MsfDataSourcePart<WorkStatusEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DspWorkStatus"/> class.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        public DspWorkStatus(params MsfDataSourcePart[] inputs)
            : base(inputs) { }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
         /// </value>
        public override string Title { get; set; } = Resources.DspWorkStatus;

        /// <summary>
        /// Generates the data.
        /// </summary>
        protected override void GenerateData()
        {
            var output = new List<WorkStatusEntity>();
            var workStatus = GetInputDataOfType<WorkStatusRawEntity>();
            var eduVars = GetInputDataOfType<EduBasedVarEntity>();

            var data = workStatus
                .Join(eduVars,
                w => new { },
                e => new { },
                (w, e) => new
                {
                    e.Education,
                    e.StartYearOfWork,
                    e.ProbabilityOfEntrantWorkStart,
                    w.WorkStatusCurrent,
                    w.WorkStatusNext,
                    w.Value
                });

            foreach (var d in data)
            {
                for (int a = 0; a <= Settings.AgeLimit; a++)
                {
                    decimal? currentValue = 0;
                    if (a < d.StartYearOfWork)
                        currentValue = d.WorkStatusNext == WorkStatus.G ? 1 : 0;
                    else if (a == d.StartYearOfWork)
                        switch (d.WorkStatusNext)
                        {
                            case WorkStatus.A:
                                currentValue = d.ProbabilityOfEntrantWorkStart;
                                break;
                            case WorkStatus.G:
                                currentValue = 1 - d.ProbabilityOfEntrantWorkStart;
                                break;
                            default:
                                currentValue = 0;
                                break;
                        }
                    else
                        currentValue = d.Value;

                    output.Add(new WorkStatusEntity()
                    {
                        Age = a,
                        Education = d.Education,
                        WorkStatusCurrent = d.WorkStatusCurrent,
                        WorkStatusNext = d.WorkStatusNext,
                        Value  = currentValue
                    });
                }
            }

            Data = output;
        }
    }
}
