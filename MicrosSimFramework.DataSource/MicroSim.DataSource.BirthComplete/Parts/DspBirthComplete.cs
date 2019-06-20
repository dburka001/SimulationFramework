using MicroSim.DataSource.Core;
using MicroSim.DataSource.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.BirthComplete
{
    /// <summary>
    /// DspBirthComplete class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSourcePart" />
    public class DspBirthComplete : MsfDataSourcePart<BirthCompleteEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DspBirthComplete"/> class.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        public DspBirthComplete(params MsfDataSourcePart[] inputs) 
            : base(inputs) { }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.DspBirthComplete;

        /// <summary>
        /// Generates the data.
        /// </summary>
        protected override void GenerateData()
        {
            var rawData = GetInputDataOfType<BirthCompleteRawEntity>();

            var unknowns = rawData.Where(d => d.Education == Resources.Unknown);

            foreach (var u in unknowns)
            {
                if (u.Value == null || u.Value == 0) continue;

                var currentData = rawData
                    .Where(d =>
                    d.AgeInterval == u.AgeInterval &&
                    d.Year == u.Year &&
                    d.NumberOfChildren == u.NumberOfChildren &&
                    d.Education != Resources.Unknown);

                decimal? sum = currentData.Sum(d => d.Value);
                if (sum == null || sum == 0) continue;

                foreach (var c in currentData)
                {
                    if (c.Value == null) continue;
                    var extraValue = (decimal)(u.Value * c.Value / sum);
                    if (c.Value == null) c.Value = extraValue;
                    else c.Value += extraValue;
                }
            }            

            Data = BirthCompleteHelper<BirthCompleteEntity>.DistributeValues(
                rawData.Except(unknowns),
                GetInputDataOfType<BirthEduBaseEntity>());
        }
    }
}
