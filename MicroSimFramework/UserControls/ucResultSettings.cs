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
    public partial class ucResultSettings : MicroSimUserControl
    {
        BlocklyBuilder cb;

        public ucResultSettings()
        {
            InitializeComponent();
            this.Title = "Results";
            cb = new cbResultSettings();
        }

        public override bool FinalizeAdjustments()
        {
            if (!ModelWebControl.Instance.IsReady) return false;
            ModelSettings.Instance.XML_ResultSettings = cb.GetBlockly();
            ModelSettings.Instance.Code_ResultSettings = cb.GetCode();
            return base.FinalizeAdjustments();
        }
    }
}