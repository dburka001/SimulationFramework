using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MicroSim.DataSource.Core
{
    /// <summary>
    /// IBaseUC interface
    /// </summary>
    public interface IBaseUC<T>
    {
        /// <summary>
        /// Loads the data.
        /// </summary>
        /// <param name="data">The data.</param>
        void LoadData(IEnumerable<T> data);
    }
}
