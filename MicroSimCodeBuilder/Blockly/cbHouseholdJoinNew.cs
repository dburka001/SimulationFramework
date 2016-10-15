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
    public partial class cbHouseholdJoinNew : BlocklyBuilder
    {
        public cbHouseholdJoinNew()
        {
            this.defaultXML = ModelSettings.Instance.XML_HouseholdJoinNew;
        }

        protected override void createBlocks()
        {                            
            AddCategory("Person");            
            AddBlock("Person", "Person_properties_get", createPersonProperties("Person", false));
            AddBlock("Person", "Person_properties_set", createPersonProperties("Person", true));
            
            AddCategory("Matrices");
            foreach (ParameterTable table in ModelData.Instance.Parameters.ValószínűségiTáblák)
            {
                AddBlock("Matrices", table.Name,
                    createArraySelector(table.Name, table.Name, table, 0, ""));
            }

            if (ModelSettings.Instance.UseHouseholds)
            {
                if (ModelSettings.Instance.RelationshipTypes.Count > 0)
                {
                    AddCategory("Relationship Type");
                    AddBlock("Relationship Type", "relationship_selector", createRelationshipTypes());
                }
                if (ModelSettings.Instance.HouseholdFields.Count > 0)
                {
                    AddCategory("New Household");
                    AddBlock("New Household", "NewHousehold_properties_get", createPersonProperties("NewHousehold", false));
                    AddBlock("New Household", "NewHousehold_properties_set", createPersonProperties("NewHousehold", true));
                }
                AddCategory("Members");
                AddBlock("Members", "member_loop", createMemberLoop());
                AddBlock("Members", "Member_properties_get", createPersonProperties("Member", false));
                AddBlock("Members", "Member_properties_set", createPersonProperties("Member", true));
                AddBlock("Members", "member_follow", createMemberFollow());                
            }

            saveCode("HouseholdJoinNew.js.log");
        }

        private StringBuilder createMemberFollow()
        {
            StringBuilder code = new StringBuilder();
            // Block
            code.AppendLine("Blockly.Blocks['member_follow'] = {");
            code.AppendLine("init: function() {");
            code.AppendLine("        this.appendDummyInput()");
            code.AppendLine("            .appendField(\"Member Follow\");");
            code.AppendLine("        this.setPreviousStatement(true, null);");
            code.AppendLine("        this.setNextStatement(true, null);");
            code.AppendLine("        this.setColour(120);");
            code.AppendLine("        this.setTooltip('Move the current member to the new household together with the person moving');");
            code.AppendLine("    }");
            code.AppendLine("};");

            // Code generator
            code.AppendLine("Blockly.CSharp['member_follow'] = function(block) {");
            code.AppendLine("    var code = 'ExtensionMethods.JoinHousehold(member, n);\\n';");
            code.AppendLine("    return code;");
            code.AppendLine("};");

            return code;
        }

        private StringBuilder createRelationshipTypes()
        {
            StringBuilder code = new StringBuilder();
            // Block
            code.AppendLine("Blockly.Blocks['relationship_selector'] = {");
            code.AppendLine("init: function() {");
            code.AppendLine("        this.appendDummyInput()");
            code.AppendLine("            .appendField(\"if the type of relationship is \")");
            code.Append("            .appendField(new Blockly.FieldDropdown([[\"Single\", \"Single\"]");
            for (int i = 0; i < ModelSettings.Instance.RelationshipTypes.Count; i++)
            {
                code.Append(", [\"" + ModelSettings.Instance.RelationshipTypes[i].Name + "\", \"" + ModelSettings.Instance.RelationshipTypes[i].Name + "\"]");
            }
            code.AppendLine("]), \"type\");");
            code.AppendLine("        this.appendStatementInput(\"innercode\")");
            code.AppendLine("            .setCheck(null);");
            code.AppendLine("        this.setPreviousStatement(true, null);");
            code.AppendLine("        this.setNextStatement(true, null);");
            code.AppendLine("        this.setColour(290);");
            code.AppendLine("        this.setTooltip('');");
            code.AppendLine("    }");
            code.AppendLine("};");

            // Code generator
            code.AppendLine("Blockly.CSharp['relationship_selector'] = function(block) {");
            code.AppendLine("    var dropdown_type = block.getFieldValue('type');");
            code.AppendLine("    var statements_innercode = Blockly.CSharp.statementToCode(block, 'innercode');");
            code.AppendLine("    var code = '';");
            code.AppendLine("    code += 'if(joinType == \"' + dropdown_type + '\")\\n';");
            code.AppendLine("    code += '{\\n';");
            code.AppendLine("    code += statements_innercode;");
            code.AppendLine("    code += '}\\n';");
            code.AppendLine("    return code;");
            code.AppendLine("};");

            return code;
        }
    }
}
