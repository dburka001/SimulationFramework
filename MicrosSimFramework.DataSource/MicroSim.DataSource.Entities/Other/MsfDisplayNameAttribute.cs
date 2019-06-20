using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.Entities
{
    /// <summary>
    /// MsfDisplayNameAttribute class
    /// </summary>
    /// <seealso cref="System.ComponentModel.DisplayNameAttribute" />
    public class MsfDisplayNameAttribute : DisplayNameAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MsfDisplayNameAttribute"/> class.
        /// </summary>
        /// <param name="resourceId">The resource identifier.</param>
        public MsfDisplayNameAttribute(string resourceId)
            : base(GetMessageFromResource(resourceId))
        { }

        /// <summary>
        /// Gets the message from resource.
        /// </summary>
        /// <param name="resourceId">The resource identifier.</param>
        /// <returns></returns>
        private static string GetMessageFromResource(string resourceId)
        {
            return Resources.ResourceManager.GetString(resourceId);
        }
    }
}
