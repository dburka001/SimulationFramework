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

namespace MicroSimCodeBuilder
{
    public partial class cbNewBorn : BlocklyBuilder
    {
        public cbNewBorn()
        {
            this.defaultXML = ModelSettings.Instance.XML_NewBorn;
        }

        protected override void createBlocks()
        {            
            AddCategory("Mother");
            AddBlock("Mother", "Mother_properties_get", createPersonProperties("Mother", false));
            // AddBlock("Mother", "Mother_properties_set", createPersonProperties("Mother", true));

            AddCategory("NewBorn");
            AddBlock("NewBorn", "NewBorn_properties_get", createPersonProperties("NewBorn", false));
            AddBlock("NewBorn", "NewBorn_properties_set", createPersonProperties("NewBorn", true));

            saveCode("NewBorn.js.log");
        }
    }
}
