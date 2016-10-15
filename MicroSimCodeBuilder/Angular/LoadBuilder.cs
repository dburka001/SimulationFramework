using MicroSimSettings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MicroSimCodeBuilder
{
    public class LoadBuilder : AngularBuilder
    {
        public LoadBuilder() : base("Load") { }
    }
    
    public class LoadBinding : AngularBinding
    {
        public void OnBrowseClicked(string type)
        {
            OnClick(ofdStart, type);
        }

        public void ofdStart(object type)
        {         
            OpenFileDialog ofd = new OpenFileDialog();
            string currentFileName = "";

            switch ((string)type)
            {
                case "Population":
                    currentFileName = ModelSettings.Instance.PopulationDataFileName;
                    ofd.Filter = "CSV files (*.csv)|*.csv";
                    break;
                case "Parameter":
                    currentFileName = ModelSettings.Instance.ParametersFileName;
                    ofd.Filter = "Excel files (*.xlsx)|*.xlsx";
                    break;
                default:
                    break;
            }
            ofd.InitialDirectory = Path.GetDirectoryName(currentFileName);
            ofd.FileName = Path.GetFileName(currentFileName);
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                switch ((string)type)
                {
                    case "Population":
                        ModelSettings.Instance.PopulationDataFileName = ofd.FileName;
                        break;
                    case "Parameter":
                        ModelSettings.Instance.ParametersFileName = ofd.FileName;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
