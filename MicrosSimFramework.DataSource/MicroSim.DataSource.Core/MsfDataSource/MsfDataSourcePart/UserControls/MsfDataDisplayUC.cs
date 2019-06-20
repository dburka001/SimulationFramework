using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MicroSim.DataSource.Core
{
    /// <summary>
    /// Default User Control for displaying an MicroSimFramework DataSource
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfBaseUC" />
    public partial class MsfDataDisplayUC : UserControl, IBaseUC<object>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MsfDataDisplayUC"/> class.
        /// </summary>
        public MsfDataDisplayUC()
        {
            InitializeComponent();
            Dock = DockStyle.Fill;
        }

        /// <summary>
        /// Loads the data.
        /// </summary>
        public void LoadData(IEnumerable<object> data)
        {
            DataGridView.DataSource = data;
        }
    }
}
