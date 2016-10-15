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
    public partial class cbSimStep : BlocklyBuilder
    {
        public cbSimStep()
        {
            this.defaultXML = ModelSettings.Instance.XML_SimStep;
        }

        protected override void createBlocks()
        {                            
            AddCategory("Person");            
            AddBlock("Person", "Person_properties_get", createPersonProperties("Person", false));
            AddBlock("Person", "Person_properties_set", createPersonProperties("Person", true));
            AddBlock("Person", "die", createFunction("die", "Person Die", 0, "The current person dies"));
            AddBlock("Person", "born", createFunction("born", "Person Born", 0, "The current person gives birth (newborn properties can be set in the next panel)"));
            AddBlock("Person", "leave", createFunction("leave", "Person Leave Household", 0, "The current leaves his household and joins a new one"));

            AddCategory("Matrices");
            foreach (ParameterTable table in ModelData.Instance.Parameters.ValószínűségiTáblák)
            {
                AddBlock("Matrices", table.Name,
                    createArraySelector(table.Name, table.Name, table, 0, ""));
            }

            if (ModelSettings.Instance.UseHouseholds)
            {
                if(ModelSettings.Instance.RelationshipTypes.Count > 0)
                    AddBlock("Person", "relationship_selector", createRelationship(0));
                AddCategory("Members");
                AddBlock("Members", "member_loop", createMemberLoop());
                AddBlock("Members", "Member_properties_get", createPersonProperties("Member", false));
            }

            saveCode("SimStep.js.log");
        }

        private StringBuilder createRelationship(int color)
        {
            StringBuilder code = new StringBuilder();
            // Block
            code.AppendLine("Blockly.Blocks['relationship_selector'] = {");
            code.AppendLine("init: function() {");
            code.AppendLine("        this.appendDummyInput()");
            code.AppendLine("            .appendField(\"Person get into relationship of type \")");
            code.Append("            .appendField(new Blockly.FieldDropdown([");
            for (int i = 0; i < ModelSettings.Instance.RelationshipTypes.Count; i++)
            {
                if (i != 0) code.Append(", ");
                code.Append("[\"" + ModelSettings.Instance.RelationshipTypes[i].Name + "\", \"" + ModelSettings.Instance.RelationshipTypes[i].Name + "\"]");
            }
            code.AppendLine("]), \"type\");");
            code.AppendLine("        this.setPreviousStatement(true, null);");
            code.AppendLine("        this.setNextStatement(true, null);");
            code.AppendLine("        this.setColour(" + color.ToString() + ");");
            code.AppendLine("        this.setTooltip('');");
            code.AppendLine("    }");
            code.AppendLine("};");

            // Code generator
            code.AppendLine("Blockly.CSharp['relationship_selector'] = function(block) {");
            code.AppendLine("    var dropdown_type = block.getFieldValue('type');");
            code.AppendLine("    var code = 'output.NewRelationship = new Relationship(currentID, p, \"' + dropdown_type + '\");\\n';");
            code.AppendLine("    return code;");
            code.AppendLine("};");

            return code;
        }
    }
}
