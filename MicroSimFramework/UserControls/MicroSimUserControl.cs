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
using System.Reflection;

namespace MicroSimFramework
{
    public partial class MicroSimUserControl : UserControl
    {
        protected ModelSettings model = ModelSettings.Instance;       
        public string Title { get; set; }

        public MicroSimUserControl()
        {
            InitializeComponent();
            this.Controls.Add(ModelWebControl.Instance);
        }

        public virtual bool FinalizeAdjustments()
        {
            this.Controls.Clear();
            this.Dispose();
            return true;
        }

        // TODO delete
        protected void setupDataFieldComboBox(ComboBox currentComboBox)
        {
            List<DataColumn> defaultFields = new List<DataColumn>();
            foreach (DataColumn dc in ModelData.Instance.PopulationData.Columns) { defaultFields.Add(dc); }
            currentComboBox.DataSource = defaultFields;
            currentComboBox.DisplayMember = "ColumnName";
        }
    }
}
