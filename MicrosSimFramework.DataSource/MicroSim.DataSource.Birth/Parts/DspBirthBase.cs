using MicroSim.DataSource.Core;
using MicroSim.DataSource.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.Birth
{
    /// <summary>
    /// DspBirthBase class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSourcePart" />
    public class DspBirthBase : MsfDataSourcePart<BirthBaseEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DspBirthBase"/> class.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        public DspBirthBase(params MsfDataSourcePart[] inputs) 
            : base(inputs) { }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.DspBirthBase;

        /// <summary>
        /// Generates the data.
        /// </summary>
        protected override void GenerateData()
        {
            var Birth = new List<BirthBaseEntity>();
            var BirthRaw = GetInputDataOfType<BirthRawEntity>();            

            foreach (var pr in BirthRaw)
            {
                var pb = new BirthBaseEntity();

                pb.BirthOrder = BirthOrderExtensions.Parse(pr.BirthOrder);                
                if (pb.BirthOrder.IsFiltered())
                    continue;
                pb.Gender = Gender.Female;
                pb.Age = Convert.ToInt32(pr.Age);
                pb.Year = pr.Year;
                pb.Value = pr.Value;
                Birth.Add(pb);
            }
            Data = Birth;
        }
    }
}
