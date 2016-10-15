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
    public partial class ucSettings : MicroSimUserControl
    {
        SettingsBuilder sb;

        public ucSettings()
        {
            InitializeComponent();
            this.Title = "Settings";
            sb = new SettingsBuilder();

            /*
            // Top
            Panel topPanel = new Panel(); topPanel.Dock = DockStyle.Fill; topPanel.AutoScroll = true;
            List<MicroSimUserControl> ucListTop = new List<MicroSimUserControl>();            
            ucListTop.Add(new ucConstants());
            ucListTop.Add(new ucPerson());
            ucListTop.Add(new ucMainSettings());
            foreach (MicroSimUserControl uc in ucListTop)
            {
                uc.Dock = DockStyle.Left;
                topPanel.Controls.Add(uc);
            }

            // Bottom
            Panel bottomPanel = new Panel(); bottomPanel.Dock = DockStyle.Fill; bottomPanel.AutoScroll = true;
            List<MicroSimUserControl> ucListBottom = new List<MicroSimUserControl>();
            ucListBottom.Add(new ucRelationshipSettings());
            ucListBottom.Add(new ucHousehold());
            foreach (MicroSimUserControl uc in ucListBottom)
            {
                uc.Dock = DockStyle.Left;
                bottomPanel.Controls.Add(uc);
            }

            // Layout
            tableLayoutPanel1.Controls.Add(topPanel, 0, 0);
            tableLayoutPanel1.Controls.Add(bottomPanel, 0, 1);
            */
        }

        public override bool FinalizeAdjustments()
        {
            bool isvalid = AngularBuilder.IsValid();
            if (!isvalid) return false;
            AngularBuilder.GetSettings();
            var model = ModelSettings.Instance;            
            return base.FinalizeAdjustments();
        }
    }
}
