using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MicroSimSettings;
using MicroSimCodeBuilder;
using CefSharp;

namespace MicroSimFramework
{
    public partial class ucLoad : MicroSimUserControl
    {
        ModelData data = ModelData.Instance;
        LoadBuilder lb;
        string oldPopulationFileName = "";
        string oldParametersFileName = "";

        public ucLoad()
        {
            InitializeComponent();
            this.Title = "Load data";
            oldPopulationFileName = ModelSettings.Instance.PopulationDataFileName;
            oldParametersFileName = ModelSettings.Instance.ParametersFileName;
            lb = new LoadBuilder();            
        }

        public override bool FinalizeAdjustments()
        {
            if (!ModelWebControl.Instance.IsReady) return false;

            // TODO - Check if exists
            // TODO - Progress bar    
            var compute = Task.Factory.StartNew(() =>
            {
                AngularBuilder.GetSettings();
                bool isReloaded = false;
                if (data.PopulationData == null || ModelSettings.Instance.PopulationDataFileName != oldPopulationFileName)
                {
                    data.LoadPopulationData();
                    isReloaded = true;
                }
                if (data.Parameters == null || ModelSettings.Instance.ParametersFileName != oldParametersFileName)
                {
                    data.LoadParameters();
                    isReloaded = true;
                }
                if (isReloaded)
                {
                    ModelSettings.Instance.DefaultFields = new List<DefaultField>();
                    foreach (DataColumn column in data.PopulationData.Columns)
                    {                        
                        string name = column.ColumnName;
                        string metaName = "";
                        data.Parameters.MetaDictionary.TryGetValue(name, out metaName);
                        ModelSettings.Instance.DefaultFields.Add(new DefaultField(name, metaName));
                    }
                }
            });

            Task.WaitAll(compute);

            return base.FinalizeAdjustments();
        }
    }
}
