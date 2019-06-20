using MicroSim.DataSource.Core;
using MicroSim.DataSource.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.Output
{
    /// <summary>
    /// DspOutputSocialGroupEducationType class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSourcePart" />
    public class DspOutputSocialGroupEducationType : MsfDataSourcePart<OutputSocialGroupEducationTypeEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DspOutputSocialGroupEducationType"/> class.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        public DspOutputSocialGroupEducationType(params MsfDataSourcePart[] inputs) 
            : base(inputs) { }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.DspOutputSocialGroupEducationType;

        /// <summary>
        /// Generates the data.
        /// </summary>
        protected override void GenerateData()
        {            
            var output = new List<OutputSocialGroupEducationTypeEntity>();
            var data = GetInputDataOfType<SocialGroupEducationTypeEntity>();

            foreach (var d in data)
            {
                output.Add(new OutputSocialGroupEducationTypeEntity()
                {
                    SocialGroup = d.SocialGroup,
                    DetEducationType = d.DetEducationType
                });
            }

            Data = output
                .OrderBy(o => o.SocialGroup);
        }
    }
}
