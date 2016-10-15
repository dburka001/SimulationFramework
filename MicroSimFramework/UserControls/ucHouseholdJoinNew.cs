using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MicroSimCodeBuilder;
using MicroSimSettings;

namespace MicroSimFramework
{
    public partial class ucHouseholdJoinNew : MicroSimUserControl
    {
        BlocklyBuilder cb;

        public ucHouseholdJoinNew()
        {
            InitializeComponent();
            this.Title = "Household Join";
            cb = new cbHouseholdJoinNew();
        }

        public override bool FinalizeAdjustments()
        {
            if (!ModelWebControl.Instance.IsReady) return false;
            ModelSettings.Instance.XML_HouseholdJoinNew = cb.GetBlockly();
            ModelSettings.Instance.Code_HouseholdJoinNew = cb.GetCode();
            return base.FinalizeAdjustments();
        }
    }
}
