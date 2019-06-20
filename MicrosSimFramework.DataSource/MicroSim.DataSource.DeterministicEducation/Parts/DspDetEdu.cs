using MicroSim.DataSource.Core;
using MicroSim.DataSource.Entities;
using MicroSim.DataSource.DataAccess;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.DeterministicEducation
{
    /// <summary>
    /// DspDetEdu class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSourcePart" />
    public class DspDetEdu : MsfDataSourcePart<DetEduEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DspDetEdu"/> class.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        public DspDetEdu(params MsfDataSourcePart[] inputs) 
            : base(inputs) { }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.DspDetEdu;

        /// <summary>
        /// Generates the data.
        /// </summary>
        protected override void GenerateData()
        {
            var detEduData = new List<DetEduEntity>();
            var detEduDataRaw = GetInputDataOfType<DetEduRawEntity>();            

            foreach (var der in detEduDataRaw)
            {
                var de = new DetEduEntity();
                
                de.Gender = GenderExtensions.Parse(der.Gender);
                de.MotherEducation = EducationExtensions.Parse(der.MotherEducation);
                de.ChildEducation = EducationExtensions.Parse(der.ChildEducation);                
                if (de.MotherEducation.IsFiltered() ||
                    de.ChildEducation.IsFiltered() ||
                    de.Gender.IsFiltered())
                    continue;
                de.Type = DetEducationTypeExtensions.Parse(der.Type);
                de.Value = der.Probability;
                detEduData.Add(de);
            }
            Data = detEduData;
        }
    }
}
