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
    /// DspBirthMother class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSourcePart" />
    public class DspBirthMother : MsfDataSourcePart<BirthMotherEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DspBirthMother"/> class.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        public DspBirthMother(params MsfDataSourcePart[] inputs) 
            : base(inputs) { }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.DspBirthMother;

        /// <summary>
        /// Generates the data.
        /// </summary>
        protected override void GenerateData()
        {
            var data = BirthCompleteHelper<BirthMotherEntity>.DistributeValues(
                GetInputDataOfType<BirthMotherRawEntity>(),
                GetInputDataOfType<PopulationEduEntity>());

            var birthOrders = data
                .Where(d => d.BirthOrder == BirthOrder.B1)
                .Select(d => d.BirthOrder).Distinct();

            foreach (var b in birthOrders)
            {
                var subData = data.Where(d => d.BirthOrder == b);
                MsfExtensions.DistributeDetEducationValues(subData);
            }

            Data = data;
        }
    }
}
