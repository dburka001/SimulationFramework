using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MicroSimFramework
{
    public partial class ucHouseholdId : MicroSimUserControl
    {
        BindingSource listBindingSource;

        public ucHouseholdId()
        {
            InitializeComponent();
            this.Title = "Household ID";

            listBindingSource = new BindingSource();
            listBindingSource.DataSource = model.HouseholdIdFields;

            listHouseholdId.DataSource = listBindingSource;

            setupDataFieldComboBox(cboxFields);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            listBindingSource.Remove(listHouseholdId.SelectedItem);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string currentHouseholdId = ((DataColumn)cboxFields.SelectedItem).ColumnName;
            if (currentHouseholdId != "" && !listHouseholdId.Items.Contains(currentHouseholdId))
                listBindingSource.Add(currentHouseholdId);
        }
    }
}
