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
    public partial class ucSimStep : MicroSimUserControl
    {
        BlocklyBuilder cb;

        public ucSimStep()
        {
            InitializeComponent();
            this.Title = "Simulation Step";
            cb = new cbSimStep();
        }

        public override bool FinalizeAdjustments()
        {
            if (!ModelWebControl.Instance.IsReady) return false;           
            ModelSettings.Instance.XML_SimStep = cb.GetBlockly();
            ModelSettings.Instance.Code_SimStep = cb.GetCode();           
            return base.FinalizeAdjustments();
        }
    }
}
