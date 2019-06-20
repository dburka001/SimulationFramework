using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.Rtools
{
    /// <summary>
    /// Entity for the output of the mortality forecast
    /// </summary>
    public class MortalityForecastEntity
    {
        public double[,] Matrix { get; set; }
        public double[] KtLower { get; set; }
        public double[] Kt { get; set; }
        public double[] KtUpper { get; set; }
    }
}
