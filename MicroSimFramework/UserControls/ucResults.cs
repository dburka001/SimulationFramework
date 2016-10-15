using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MicroSimResults;
using MicroSimSettings;
using System.Reflection;

namespace MicroSimFramework
{
    public partial class ucResults : MicroSimUserControl
    {
        public ucResults()
        {
            InitializeComponent();
            this.Title = "Results";
            this.Controls.Remove(ModelWebControl.Instance);
            
            if (!ModelData.Instance.AgeTreeResults.Any()) return;
            BindingSource resultBindingSource = new BindingSource(ModelData.Instance.AgeTreeResults, null);

            cboxResultList.DisplayMember = "Value";
            cboxResultList.ValueMember = "Key";
            cboxResultList.DataSource = resultBindingSource;            
        }

        public override bool FinalizeAdjustments()
        {
            return base.FinalizeAdjustments();
        }

        ucAgeTree uc = null;
        private void cboxResultList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboxResultList.Items.Count == 0) return;            
            AgeTreeInput data = (AgeTreeInput)cboxResultList.SelectedValue;
            if (uc != null)
            {
                uc.Controls.Clear();
                uc.Dispose();
            }
            mainPanel.Controls.Clear();            

            uc = new ucAgeTree(data);     
            uc.Dock = DockStyle.Fill;
            mainPanel.Controls.Add(uc);           
        }
    }
}
