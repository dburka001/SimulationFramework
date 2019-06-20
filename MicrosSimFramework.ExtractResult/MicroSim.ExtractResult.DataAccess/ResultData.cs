using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.ExtractResource.DataAccess
{
    public sealed class ResultData
    {
        public List<ResultItem> Items { get; } = new List<ResultItem>();

        /// <summary>
        /// The instance
        /// </summary>
        private static ResultData _instance;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static ResultData Instance
            => _instance ?? (_instance = new ResultData());

        /// <summary>
        /// Prevents a default instance of the <see cref="ResultData"/> class from being created.
        /// </summary>
        private ResultData()
        {}
    }
}
