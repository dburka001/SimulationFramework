using MicroSim.ExtractResource.DataAccess;
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

namespace MicroSim.ExtractResource.UI
{
    public partial class Form1 : Form
    {       
        public Form1()
        {
            InitializeComponent();
            txtFileName.DataBindings.Add(
                new Binding(
                    "Text",
                    Properties.Settings.Default,
                    "InputFilePath",
                    true,
                    DataSourceUpdateMode.OnPropertyChanged));
            DataImporter.RunTimeRead += DataImporter_RunTimeRead;
            DataImporter.Log += DataLoader_Log;
            DataExporter.Log += DataLoader_Log;
            LoadResultData();
        }

        private void DataImporter_RunTimeRead(object sender, string msg)
        {
            this.Text = msg;
        }

        private void DataLoader_Log(object sender, string msg)
        {
            txtOutput.Text += msg + Environment.NewLine;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Filter = "Simulation result files (*.sr)|*.sr",
                Title = "Open simulation result file",
            };

            var filePath = Properties.Settings.Default.InputFilePath;
            if (filePath != "")
            {                
                var dir = Path.GetDirectoryName(filePath);
                if (Directory.Exists(dir))
                    ofd.InitialDirectory = dir;
                var fileName = Path.GetFileName(filePath);
                ofd.FileName = fileName;
            }

            if (ofd.ShowDialog() != DialogResult.OK) return;

            Properties.Settings.Default.InputFilePath = ofd.FileName;            
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        private void LoadResultData()
        {
            DataImporter.LoadResultData(Properties.Settings.Default.InputFilePath);
        }

        private void btnExportAll_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            var folderPath = Properties.Settings.Default.OutputFolder;
            if (folderPath != "")
            {
                if (Directory.Exists(folderPath))
                    fbd.SelectedPath = folderPath;
            }

            if (fbd.ShowDialog() != DialogResult.OK) return;

            Properties.Settings.Default.OutputFolder = fbd.SelectedPath;

            DataExporter.SaveResultData(fbd.SelectedPath);
        }

        private void txtFileName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoadResultData();
            }
        }
    }
}
