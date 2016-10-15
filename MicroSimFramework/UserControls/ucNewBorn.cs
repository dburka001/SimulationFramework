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
    public partial class ucNewBorn : MicroSimUserControl
    {
        BlocklyBuilder cb;

        public ucNewBorn()
        {
            InitializeComponent();
            this.Title = "Newborn Properties";
            cb = new cbNewBorn();
        }

        public override bool FinalizeAdjustments()
        {
            if (!ModelWebControl.Instance.IsReady) return false;            
            ModelSettings.Instance.XML_NewBorn = cb.GetBlockly();
            ModelSettings.Instance.Code_NewBorn = cb.GetCode();            
            return base.FinalizeAdjustments();
        }
    }
}
