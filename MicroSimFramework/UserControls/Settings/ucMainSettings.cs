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

namespace MicroSimFramework
{
    public partial class ucMainSettings : MicroSimUserControl
    {        
        public ucMainSettings()
        {
            InitializeComponent();
            this.Title = "Main";

            setupDataFieldComboBox(cboxMultiplierField);

            txtStartYear.DataBindings.Add(new Binding("Text", model, "StartYear", true, DataSourceUpdateMode.OnPropertyChanged));
            txtEndYear.DataBindings.Add(new Binding("Text", model, "EndYear", true, DataSourceUpdateMode.OnPropertyChanged));
            cboxMultiplierField.DataBindings.Add(new Binding("SelectedItem", model, "MultiplierField", true, DataSourceUpdateMode.OnPropertyChanged));
            chkUseHouseholds.DataBindings.Add(new Binding("Checked", model, "UseHouseholds", true, DataSourceUpdateMode.OnPropertyChanged));
        }
    }
}
