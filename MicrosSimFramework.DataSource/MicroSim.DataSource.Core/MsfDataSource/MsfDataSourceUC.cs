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
    /// MsfDataSourceUC class;
    /// </summary>
    /// <seealso cref="System.Windows.Forms.UserControl" />
    public partial class MsfDataSourceUC : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MsfDataSourceUC"/> class.
        /// </summary>
        public MsfDataSourceUC()
        {
            InitializeComponent();
            Dock = DockStyle.Fill;
        }
    }
}
