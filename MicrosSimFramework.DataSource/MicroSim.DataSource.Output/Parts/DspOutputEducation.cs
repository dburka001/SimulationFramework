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
    /// DspOutputEducation class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSourcePart" />
    public class DspOutputEducation : MsfDataSourcePart<OutputEducationEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DspOutputEducation"/> class.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        public DspOutputEducation(params MsfDataSourcePart[] inputs) 
            : base(inputs) { }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.DspOutputEducation;

        /// <summary>
        /// Generates the data.
        /// </summary>
        protected override void GenerateData()
        {            
            var output = new List<OutputEducationEntity>();
            var data = GetInputDataOfType<DetEduEntity>();

            foreach (var d in data)
            {
                var aggregatedValue = data
                    .Where(c =>
                    c.Type == d.Type &&
                    c.Gender == d.Gender && 
                    c.MotherEducation == d.MotherEducation &&
                    (int)c.ChildEducation <= (int)d.ChildEducation)
                    .Sum(c => c.Value);

                output.Add(new OutputEducationEntity()
                {
                    EducationType = d.Type,
                    Gender = d.Gender,
                    MotherEducation = d.MotherEducation,
                    ChildEducation = d.ChildEducation,
                    Value = aggregatedValue
                });
            }            

            Data = output
                .OrderBy(o => o.EducationType)
                .ThenBy(o => o.Gender)
                .ThenBy(o => o.MotherEducation)
                .ThenBy(o => o.ChildEducation);
        }
    }
}
