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
    public partial class ucHousehold : MicroSimUserControl
    {
        public ucHousehold()
        {
            InitializeComponent();
            this.Title = "Household";

            MicroSimUserControl uc = new ucClass(model.HouseholdFields);
            uc.Dock = DockStyle.Left;
            this.Controls.Add(uc);

            MicroSimUserControl uc2 = new ucHouseholdId();
            uc2.Dock = DockStyle.Left;
            this.Controls.Add(uc2);
        }
    }
}
