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
using System.IO;

namespace MicroSimCodeBuilder
{
    public partial class cbResultSettings : BlocklyBuilder
    {
        public cbResultSettings()
        {      
            this.defaultXML = ModelSettings.Instance.XML_ResultSettings;
        }

        protected override void createBlocks()
        {
            AddCategory("Person");
            AddBlock("Person", "Person_properties_get", createPersonProperties("Person", false));

            if(ModelSettings.Instance.UseHouseholds)
            {
                AddCategory("Household");
                AddBlock("Household", "Household_properties_get", createPersonProperties("Household", false));             
            }

            AddCategory("Select");
            AddBlock("Select", "select", createSelect("select.js"));

            if (ModelSettings.Instance.UseHouseholds)
            {
                AddBlock("Select", "selectHousehold", createSelect("selectHousehold.js"));
            }

            saveCode("ResultSettings.js.log");
        }

        private StringBuilder createSelect(string filename)
        {
            StringBuilder code = new StringBuilder();

            StreamReader sr = new StreamReader(@"Blockly\BlocklyJS\" + filename, Encoding.Default);
            while (!sr.EndOfStream)
            {
                code.AppendLine(sr.ReadLine());
            }
            sr.Close();

            return code;
        }
    }
}
