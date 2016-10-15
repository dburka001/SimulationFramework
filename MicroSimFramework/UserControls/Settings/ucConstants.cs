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
    public partial class ucConstants : MicroSimUserControl
    {        
        BindingSource listConstantsBindingSource;

        public ucConstants()
        {
            InitializeComponent();
            this.Title = "Constants";            

            listConstantsBindingSource = new BindingSource();
            listConstantsBindingSource.DataSource = model.Constants;

            listConstants.DataSource = listConstantsBindingSource;
            listConstants.ValueMember = "Name";
            listConstants.DisplayMember = "Name";

            txtName.DataBindings.Add(new Binding("Text", listConstantsBindingSource, "Name", true, DataSourceUpdateMode.OnPropertyChanged));
            chkMultipleValue.DataBindings.Add(new Binding("Checked", listConstantsBindingSource, "IsMultipleValue", true, DataSourceUpdateMode.OnPropertyChanged));
            txtFrom.DataBindings.Add(new Binding("Text", listConstantsBindingSource, "From", true, DataSourceUpdateMode.OnPropertyChanged));
            txtStep.DataBindings.Add(new Binding("Text", listConstantsBindingSource, "Step", true, DataSourceUpdateMode.OnPropertyChanged));
            txtTo.DataBindings.Add(new Binding("Text", listConstantsBindingSource, "To", true, DataSourceUpdateMode.OnPropertyChanged));
        }

        private void chkMultipleValue_CheckedChanged(object sender, EventArgs e)
        {
            if (chkMultipleValue.Checked)
            {
                txtStep.Show();
                txtTo.Show();
            }
            else
            {
                txtStep.Hide();
                txtTo.Hide();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            listConstantsBindingSource.Add(new Constant("Constant" + model.Constants.Count.ToString(), 0));
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (listConstantsBindingSource.Count == 0) return;
            listConstantsBindingSource.Remove(listConstants.SelectedItem);
        }
    }
}