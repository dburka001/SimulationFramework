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
    public partial class ucClass : MicroSimUserControl
    {
        List<ClassField> fields;
        BindingSource listBindingSource;

        public ucClass(List<ClassField> fields)
        {
            InitializeComponent();

            this.Title = "";
            this.fields = fields;

            listBindingSource = new BindingSource();
            listBindingSource.DataSource = fields;

            listProperties.DataSource = listBindingSource;
            listProperties.ValueMember = "Name";
            listProperties.DisplayMember = "Name";

            List<Type> types = new List<Type>() { typeof(string), typeof(bool), typeof(int), typeof(double), typeof(float) };
            cboxTypes.DataSource = types;
            cboxTypes.DisplayMember = "Name";            
            setupDataFieldComboBox(cboxDefaultFields);

            cboxTypes.DataBindings.Add(new Binding("SelectedItem", listBindingSource, "Type", true, DataSourceUpdateMode.OnPropertyChanged));
            txtName.DataBindings.Add(new Binding("Text", listBindingSource, "Name", true, DataSourceUpdateMode.OnPropertyChanged));
            txtDefaultValue.DataBindings.Add(new Binding("Text", listBindingSource, "DefaultValue", true, DataSourceUpdateMode.OnPropertyChanged));
            cboxDefaultFields.DataBindings.Add(new Binding("SelectedItem", listBindingSource, "DefaultDataField", true, DataSourceUpdateMode.OnPropertyChanged));
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // listBindingSource.Add(new ClassField(typeof(int), "Field" + fields.Count.ToString()));
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (listBindingSource.Count == 0) return;
            listBindingSource.Remove(listProperties.SelectedItem);
        }
    }
}
