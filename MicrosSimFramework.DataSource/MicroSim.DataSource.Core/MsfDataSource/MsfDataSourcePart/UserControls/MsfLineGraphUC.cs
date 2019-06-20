using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace MicroSim.DataSource.Core
{
    /// <summary>
    /// UserControl for displaying multiple line graphs
    /// </summary>
    /// <seealso cref="System.Windows.Forms.UserControl" />
    public partial class MsfLineGraphUC : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MsfLineGraphUC"/> class.
        /// </summary>
        public MsfLineGraphUC()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        public void Reset()
        {
            mainChart.Series.Clear();
        }

        /// <summary>
        /// Gets or sets the x label.
        /// </summary>
        /// <value>
        /// The x label.
        /// </value>
        public string xLabel { get; set; } = nameof(xLabel);

        /// <summary>
        /// Gets or sets the y label.
        /// </summary>
        /// <value>
        /// The y label.
        /// </value>
        public string yLabel { get; set; } = nameof(yLabel);

        /// <summary>
        /// The x values
        /// </summary>
        private IEnumerable<int> _xValues = null;

        /// <summary>
        /// Sets the x values.
        /// </summary>
        /// <param name="values">The values.</param>
        public void SetXValues(IEnumerable<int> values)
        {
            _xValues = values;
        }

        /// <summary>
        /// Adds the series.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="values">The values.</param>
        public void AddSeries(string name, IEnumerable<decimal?> values)
        {
            var series = new Series(name);
            if(_xValues != null)
                series.Points.DataBindXY(_xValues, values);
            else
                series.Points.DataBindY(values);
            series.BorderWidth = 3;
            series.ChartType = SeriesChartType.Line;
            mainChart.Series.Add(series);
        }

        /// <summary>
        /// MSFs the line graph uc load.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void MsfLineGraphUCLoad(object sender, EventArgs e)
        {
            mainChart.ChartAreas[0].AxisX.Title = xLabel;
            mainChart.ChartAreas[0].AxisY.Title = yLabel;
        }
    }
}
