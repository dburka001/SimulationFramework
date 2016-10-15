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
    public partial class ucPerson : MicroSimUserControl
    {
        public ucPerson()
        {
            InitializeComponent();

            this.Title = "Person Properties";
            
            MicroSimUserControl uc = new ucClass(model.PersonFields);
            uc.Dock = DockStyle.Fill;
            this.Controls.Add(uc);
        }
    }
}
