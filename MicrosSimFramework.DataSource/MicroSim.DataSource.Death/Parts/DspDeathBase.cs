using MicroSim.DataSource.Core;
using MicroSim.DataSource.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.Death
{
    /// <summary>
    /// DspDeathBase class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSourcePart" />
    public class DspDeathBase : MsfDataSourcePart<DeathBaseEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DspDeathBase"/> class.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        public DspDeathBase(params MsfDataSourcePart[] inputs) 
            : base(inputs) { }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.DspDeathBase;

        /// <summary>
        /// Generates the data.
        /// </summary>
        protected override void GenerateData()
        {
            var Death = new List<DeathBaseEntity>();
            var DeathRaw = GetInputDataOfType<DeathRawEntity>();            

            foreach (var pr in DeathRaw)
            {
                var pb = new DeathBaseEntity();

                pb.Gender = GenderExtensions.Parse(pr.Sex);
                if (pb.Gender.IsFiltered())
                    continue;                
                pb.Age = Convert.ToInt32(pr.Age);
                pb.Year = pr.Year;
                pb.Value = pr.Value;
                Death.Add(pb);
            }

            Data = Death;
        }
    }
}
