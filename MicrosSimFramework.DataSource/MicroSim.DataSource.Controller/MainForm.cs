using MicroSim.DataSource.Core;
using MicroSim.DataSource.ProcessLog;
using MicroSim.DataSource.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MicroSim.DataSource.Entities;
using MicroSim.DataSource.ExcelExport.Commands;

namespace MicroSim.DataSource.Controller
{
    /// <summary>
    /// MainForm class
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Form" />
    public partial class MainForm : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            Log.LogUpdated += LogUpdated;
            InitalizeDataSources();
        }

        /// <summary>
        /// Log updated event.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="color">The color.</param>
        private void LogUpdated(string message, Color color)
        {
            InvokeExtensions.InvokeAction(rtbLog, () => rtbLog.AppendText(message, color));
        }

        /// <summary>
        /// Initalizes the data sources.
        /// </summary>
        private void InitalizeDataSources()
        {
            foreach (var ds in MsfDataModel.Instance.DataSources)
            {
                ds.Initalize();
            }
            SetupDataSourceList();
        }

        /// <summary>
        /// Loads the data sources.
        /// </summary>
        /// <param name="dataSourcesList">The data sources list.</param>
        private async void LoadDataSources(IEnumerable<MsfDataSource> dataSourcesList, bool forceGeneration = false)
        {
            rtbLog.Clear();
            var itemCount = dataSourcesList.Sum(ds => ds.Parts.Count);
            Log.WriteLine(String.Format(Resources.LoadStared, itemCount), LogLevel.Important);
            foreach (var ds in dataSourcesList)
            {
                await Task.Factory.StartNew(() => ds.Load(forceGeneration));
            }
            Log.WriteLine(Resources.LoadFinished, LogLevel.Important);
        }

        /// <summary>
        /// Setups the data source list.
        /// </summary>
        private void SetupDataSourceList()
        {
            listMsfDataSource.DataSource = MsfDataModel.Instance.DataSources;
            listMsfDataSource.DisplayMember = "Title";
            DisplayDataSource((MsfDataSource)listMsfDataSource.SelectedItem);
        }

        /// <summary>
        /// Lists the MSF data source on selected index changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void listMsfDataSourceSelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayDataSource((MsfDataSource)listMsfDataSource.SelectedItem);
        }

        /// <summary>
        /// Displays the data source.
        /// </summary>
        /// <param name="dataSource">The data source.</param>
        private void DisplayDataSource(MsfDataSource dataSource)
        {
            mainPanel.Controls.Clear();
            mainPanel.Controls.Add(dataSource.View);
        }

        /// <summary>
        /// btnGenerateAgain clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnGenerateAgainClick(object sender, EventArgs e)
        {
            LoadDataSources(listMsfDataSource.SelectedItems.Cast<MsfDataSource>(), true);
        }

        /// <summary>
        /// Mains the form shown.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void MainFormShown(object sender, EventArgs e)
        {
            LoadDataSources(MsfDataModel.Instance.DataSources);
        }

        /// <summary>
        /// BTNs the export click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnExportClick(object sender, EventArgs e)
            => Task.Factory.StartNew(() => DoExportAsync());

        private async Task DoExportAsync()
        {
            BeginInvoke((Action)(() => btnExport.Enabled = false));

            await MsfDataModel.Instance.ExportAsync(
                new XlsxExportCommand()
                );

            BeginInvoke((Action)(() => btnExport.Enabled = true));
        }
    }
}
