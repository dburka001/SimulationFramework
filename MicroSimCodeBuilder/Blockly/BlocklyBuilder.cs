using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp.WinForms;
using CefSharp;
using System.IO;
using MicroSimSettings;

namespace MicroSimCodeBuilder
{
    public partial class BlocklyBuilder
    {
        private ModelWebControl browser;
        protected bool isGlobalRandom = false;        
        protected string defaultXML;
        protected StringBuilder jsCode = new StringBuilder();

        public BlocklyBuilder()
        {            
            browser = ModelWebControl.Instance;
            browser.IsReady = false;
            browser.Load(Path.Combine(Environment.CurrentDirectory, @"Blockly\BlocklyJS\index.html"));         
            browser.FrameLoadEnd += Browser_FrameLoadEnd;
        }

        private void Browser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            createDefaultBlocks();
            createBlocks();
            createWorkSpace();
            browser.GetMainFrame().ExecuteJavaScriptAsync(jsCode.ToString());
            SetBlockly(defaultXML);
            browser.FrameLoadEnd -= Browser_FrameLoadEnd;            
        }

        protected virtual void createBlocks() { }        
        
        private void createWorkSpace()
        {
            jsCode.Append("var workspace = Blockly.inject('blocklyDiv',");
            jsCode.Append("          {comments: true,");
            jsCode.Append("           disable: true,");
            jsCode.Append("           collapse: true,");
            jsCode.Append("           grid:");
            jsCode.Append("             {spacing: 25,");
            jsCode.Append("              length: 3,");
            jsCode.Append("              colour: '#ccc',");
            jsCode.Append("              snap: true},");
            jsCode.Append("           maxBlocks: Infinity,");
            jsCode.Append("           media: './media/',");
            jsCode.Append("           readOnly: false,");
            jsCode.Append("           scrollbars: true,");
            jsCode.Append("           toolbox: document.getElementById('toolbox'),");
            jsCode.Append("           zoom:");
            jsCode.Append("             {controls: true,");
            jsCode.Append("              wheel: true,");
            jsCode.Append("              startScale: 1.0,");
            jsCode.Append("              maxScale: 4,");
            jsCode.Append("              minScale: .25,");
            jsCode.Append("              scaleSpeed: 1.1}");
            jsCode.Append("          });");

        }

        protected void saveCode(string fileName)
        {
            StreamWriter sw = new StreamWriter(fileName, false, Encoding.Default);
            sw.Write(jsCode.ToString());
            sw.Close();
        }

        private void createDefaultBlocks()
        {
            AddCategory("Variables"); // TODO playground     
            AddBlock("Variables", "year", createInput("year", "Year", 330, "The current year of the simulation"));
            AddBlock("Variables", "variables_set");
            AddBlock("Variables", "variables_get");

            if (ModelSettings.Instance.Constants.Count > 0)
            {
                AddCategory("Constants");
                foreach (Constant c in ModelSettings.Instance.Constants)
                {
                    AddBlock("Constants", c.Name, createInput(c.Name, c.Name, 330, c.Description));
                }
            }

            AddCategory("Logic");
            AddBlock("Logic", "controls_if");
            AddBlock("Logic", "logic_compare");
            AddBlock("Logic", "logic_operation");
            AddBlock("Logic", "logic_negate");
            AddBlock("Logic", "logic_boolean");
            AddBlock("Logic", "logic_null");
            AddBlock("Logic", "logic_ternary");

            AddCategory("Math");
            AddBlock("Math", "math_number");
            AddBlock("Math", "math_arithmetic");
            AddBlock("Math", "math_constant");
            AddBlock("Math", "math_on_list");
            AddBlock("Math", "math_modulo");
            AddBlock("Math", "math_constrain");

            AddBlock("Math", "math_random_int_bd", createRandom(isGlobalRandom));
            AddBlock("Math", "math_random_float_bd", createRandomFraction(isGlobalRandom));

            AddCategory("Comments");
            AddBlock("Comments", "comment1", createComments(1));
            AddBlock("Comments", "comment2", createComments(2));

            AddCategory("Nomenclature");
            foreach (Nómenklatúra nomenclature in ModelData.Instance.Parameters.Nómenklatúrák)
            {
                AddBlock("Nomenclature", nomenclature.Name,
                    createInput(nomenclature.Name, (nomenclature.MetaName != null ? nomenclature.MetaName : nomenclature.Name), nomenclature, 0, ""));
                //AddBlock("Nomenclature", nomenclature.Name + "_converter", createNomenclatureConverter(nomenclature, 0));
            }
        }

        public string GetCode()
        {
            string code = @"(function() { return window.Blockly.CSharp.workspaceToCode(window.Blockly.mainWorkspace); })();";
            string resultString = (browser.GetMainFrame().EvaluateScriptAsync(code, null)).Result.Result.ToString();
            return resultString;            
        }

        public string GetBlockly()
        {
            string code = @"(function() { return window.Blockly.Xml.domToPrettyText(window.Blockly.Xml.workspaceToDom(window.Blockly.mainWorkspace)); })();";
            string resultString = (browser.GetMainFrame().EvaluateScriptAsync(code, null)).Result.Result.ToString();
            return resultString;
        }

        public void SetBlockly(string xml)
        {            
            if (xml == null) return;
            xml = Convert.ToBase64String(Encoding.UTF8.GetBytes(xml));
            string input = "if (window.Blockly.mainWorkspace != null) { window.Blockly.mainWorkspace.clear(); xml = window.Blockly.Xml.textToDom(base64.decode(\"" + xml + "\")); window.Blockly.Xml.domToWorkspace(xml, window.Blockly.mainWorkspace); }";
            browser.GetMainFrame().ExecuteJavaScriptAsync(input);
        }

        protected void AddCategory(string category)
        {
            jsCode.AppendLine("var c = document.createElement('category');");
            jsCode.AppendLine("c.setAttribute('name', '" + category + "');");
            jsCode.AppendLine("document.getElementById('toolbox').appendChild(c);");
            jsCode.AppendLine();
        }

        protected void AddBlock(string category, string blockName)
        {
            AddBlockMain(category, blockName, null);
        }

        protected void AddBlock(string category, string blockName, StringBuilder blockCode)
        {
            AddBlockMain(category, blockName, blockCode);
        }

        private void AddBlockMain(string category, string blockName, StringBuilder blockCode)
        {
            jsCode.AppendLine("var e = document.createElement('block');");
            jsCode.AppendLine("e.setAttribute('type', '" + blockName + "');");
            jsCode.AppendLine("document.getElementsByName('" + category + "')[0].appendChild(e);");
            jsCode.AppendLine();
            if (blockCode != null)
            {
                jsCode.Append(blockCode.ToString());
                jsCode.AppendLine();
            }
        }

        private StringBuilder createRandom(bool isGlobal)
        {
            StringBuilder code = new StringBuilder();

            // Block            
            code.AppendLine("Blockly.Blocks['math_random_int_bd'] = {");
            code.AppendLine("init: function() {");
            code.AppendLine("        this.appendDummyInput()");
            code.AppendLine("            .appendField(\"random integer from \");");
            code.AppendLine("        this.appendValueInput(\"FROM\")");
            code.AppendLine("            .setCheck(null);");
            code.AppendLine("        this.appendDummyInput()");
            code.AppendLine("            .appendField(\"to \");");
            code.AppendLine("        this.appendValueInput(\"TO\")");
            code.AppendLine("            .setCheck(null);");
            code.AppendLine("        this.setInputsInline(true);");
            code.AppendLine("        this.setOutput(true, null);");
            code.AppendLine("        this.setColour(230);");
            code.AppendLine("        this.setTooltip('');");
            code.AppendLine("    }");
            code.AppendLine("};");
            code.AppendLine();

            // Code generator
            code.AppendLine("Blockly.CSharp.math_random_int_bd = function() {");
            code.AppendLine("    var argument0 = Blockly.CSharp.valueToCode(this, 'FROM',");
            code.AppendLine("        Blockly.CSharp.ORDER_COMMA) || '0.0';");
            code.AppendLine("    var argument1 = Blockly.CSharp.valueToCode(this, 'TO',");
            code.AppendLine("        Blockly.CSharp.ORDER_COMMA) || '0.0';");
            code.AppendLine("    if (!Blockly.CSharp.definitions_['math_random_int_bd'])");
            code.AppendLine("    {");
            code.AppendLine("        var functionName = Blockly.CSharp.variableDB_.getDistinctName(");
            code.AppendLine("            'math_random_int_bd', Blockly.Generator.NAME_TYPE);");
            code.AppendLine("        Blockly.CSharp.math_random_int_bd.random_function = functionName;");
            code.AppendLine("        var func = [];");
            code.AppendLine("        func.push('var ' + functionName + ' new Func<int,int,int>((a, b) => {');");
            code.AppendLine("        func.push('  if (a > b) {');");
            code.AppendLine("        func.push('    // Swap a and b to ensure a is smaller.');");
            code.AppendLine("        func.push('    var c = a;');");
            code.AppendLine("        func.push('    a = b;');");
            code.AppendLine("        func.push('    b = c;');");
            code.AppendLine("        func.push('  }');");
            code.AppendLine("        func.push('  return (int)Math.Floor(a + " + (isGlobal ? "GlobalRng" : "rng") + ".Next(b - a));');");
            code.AppendLine("        func.push('});');");
            code.AppendLine("        Blockly.CSharp.definitions_['math_random_int_bd'] = func.join('\\n');");
            code.AppendLine("    }");
            code.AppendLine("    var code = Blockly.CSharp.math_random_int_bd.random_function +");
            code.AppendLine("        '(' + argument0 + ', ' + argument1 + ')';");
            code.AppendLine("    return [code, Blockly.CSharp.ORDER_FUNCTION_CALL];");
            code.AppendLine("};");

            return code;
        }

        private StringBuilder createRandomFraction(bool isGlobal)
        {
            StringBuilder code = new StringBuilder();

            // Block            
            code.AppendLine("Blockly.Blocks['math_random_float_bd'] = {");
            code.AppendLine("init: function() {");
            code.AppendLine("        this.appendDummyInput()");
            code.Append("            .appendField(\"random fraction\")");
            code.AppendLine(";");
            code.AppendLine("        this.setOutput(true, null);");
            code.AppendLine("        this.setColour(230);");
            code.AppendLine("        this.setTooltip('');");
            code.AppendLine("    }");
            code.AppendLine("};");
            code.AppendLine();

            // Code generator
            code.AppendLine("Blockly.CSharp.math_random_float_bd = function() {");
            code.AppendLine("    return ['" + (isGlobal ? "GlobalRng" : "rng") + ".NextDouble()', Blockly.CSharp.ORDER_FUNCTION_CALL];");
            code.AppendLine("};");

            return code;
        }

        protected StringBuilder createInput(string name, string label, int color, string tooltip)
        {
            return createDummyInput(name, label, null, color, tooltip);
        }

        protected StringBuilder createInput(string name, string label, Nómenklatúra nomenclature, int color, string tooltip)
        {
            return createDummyInput(name, label, nomenclature, color, tooltip);
        }

        private StringBuilder createDummyInput(string name, string label, Nómenklatúra nomenclature, int color, string tooltip)
        {
            StringBuilder code = new StringBuilder();

            // Block            
            code.AppendLine("Blockly.Blocks['" + name + "'] = {");
            code.AppendLine("init: function() {");
            code.AppendLine("        this.appendDummyInput()");
            code.Append("            .appendField(\"" + label + "\")");
            if (nomenclature != null && nomenclature.Elemek.Count > 0)
            {

                code.Append("            .appendField(new Blockly.FieldDropdown([");
                for (int i = 0; i < nomenclature.Elemek.Count; i++)
                {
                    NómentklatúraElem nItem = nomenclature.Elemek[i];
                    code.Append("[\"" + nItem.OriginalName + "\",\"" + nItem.CodeName + "\"]");
                    if (i != nomenclature.Elemek.Count - 1) code.Append(", ");
                }
                code.Append("]), \"" + name + "\")");
            }
            code.AppendLine(";");
            code.AppendLine("        this.setOutput(true, null);");
            code.AppendLine("        this.setColour(" + color.ToString() + ");");
            code.AppendLine("        this.setTooltip('" + tooltip + "');");
            code.AppendLine("    }");
            code.AppendLine("};");
            code.AppendLine();

            // Code generator
            code.AppendLine("Blockly.CSharp['" + name + "'] = function(block) {");
            if (nomenclature != null) code.AppendLine("    var code = '(int)" + name + ".' + this.getFieldValue('" + name + "');");
            else code.AppendLine("    var code = '" + label + "';");
            code.AppendLine("    return [code, Blockly.CSharp.ORDER_ATOMIC];");
            code.AppendLine("};");

            return code;
        }

        // TODO - Not working right now and not used
        private StringBuilder createNomenclatureConverter(Nómenklatúra nomenclature, int color)
        {
            StringBuilder code = new StringBuilder();

            // Block            
            code.AppendLine("Blockly.Blocks['" + nomenclature.Name + "_converter'] = {");
            code.AppendLine("init: function() {");
            code.AppendLine("        this.appendValueInput(\"value\")");
            code.AppendLine("            .setCheck(null)");
            code.AppendLine("            .appendField(\"" + (nomenclature.MetaName != "" ? nomenclature.MetaName : nomenclature.Name) + "\");");
            code.AppendLine("        this.setInputsInline(false);");
            code.AppendLine("        this.setOutput(true, null);");
            code.AppendLine("        this.setColour(" + color.ToString() + ");");
            code.AppendLine("        this.setTooltip('');");
            code.AppendLine("    }");
            code.AppendLine("};");
            code.AppendLine();

            // Code generator
            code.AppendLine("Blockly.CSharp['" + nomenclature.Name + "_converter'] = function(block) {");
            code.AppendLine("var value_value = Blockly.CSharp.valueToCode(block, 'value', Blockly.JavaScript.ORDER_ATOMIC);");
            code.AppendLine("    var code = '(" + nomenclature.Name + ") + value_value';");
            code.AppendLine("    return [code, Blockly.CSharp.ORDER_ATOMIC];");
            code.AppendLine("};");

            return code;
        }

        protected StringBuilder createFunction(string name, string label, int color, string tooltip)
        {
            StringBuilder code = new StringBuilder();

            // Block
            code.AppendLine("Blockly.Blocks['" + name + "'] = {");
            code.AppendLine("init: function() {");
            code.AppendLine("        this.appendDummyInput()");
            code.Append("            .appendField(\"" + label + "\")");
            code.AppendLine(";");
            code.AppendLine("        this.setPreviousStatement(true, null);");
            code.AppendLine("        this.setNextStatement(true, null);");
            code.AppendLine("        this.setColour(" + color.ToString() + ");");
            code.AppendLine("        this.setTooltip('" + tooltip + "');");
            code.AppendLine("    }");
            code.AppendLine("};");

            // Code generator
            code.AppendLine("Blockly.CSharp['" + name + "'] = function(block) {");
            code.AppendLine("    var code = '" + label.Replace("Person", "p.").Replace(" ", "") + "();\\n';");
            code.AppendLine("    return code;");
            code.AppendLine("};");

            return code;
        }

        protected StringBuilder createArraySelector(string name, string label, ParameterTable table, int color, string tooltip)
        {
            StringBuilder code = new StringBuilder();

            // Block
            code.AppendLine("Blockly.Blocks['" + name + "'] = {");
            code.AppendLine("init: function() {");
            code.AppendLine("        this.appendDummyInput()");
            code.AppendLine("            .appendField(\"" + label + "\");");
            for (int i = 0; i < table.OszlopNevek.Count - 1; i++)
            {
                code.AppendLine("        this.appendValueInput(\"param" + i.ToString() + "\")");
                code.AppendLine("            .setCheck(null)");
                code.AppendLine("            .setAlign(Blockly.ALIGN_RIGHT)");
                code.AppendLine("            .appendField(\"" + table.OszlopNevek[i] + "\");");
            }
            code.AppendLine("        this.setOutput(true, null);");
            code.AppendLine("        this.setColour(" + color.ToString() + ");");
            code.AppendLine("        this.setTooltip('" + tooltip + "');");
            code.AppendLine("    }");
            code.AppendLine("};");

            // Code generator
            code.AppendLine("Blockly.CSharp['" + name + "'] = function(block) {");
            code.Append("    var code = 'ExtensionMethods." + name + "(");
            for (int i = 0; i < table.OszlopNevek.Count - 1; i++)
            {
                if (table.nómenklatúrák[i] != null) code.Append("(" + table.nómenklatúrák[i].Name + ")");
                else code.Append("(int)");
                code.Append("' + Blockly.CSharp.valueToCode(this, 'param" + i.ToString() + "') + '");
                if (i != table.OszlopNevek.Count - 2) code.Append(", ");
            }
            code.AppendLine(")';");
            code.AppendLine("    return [code, Blockly.CSharp.ORDER_ATOMIC];");
            code.AppendLine("};");

            return code;
        }

        protected StringBuilder createComments(int type)
        {
            StringBuilder code = new StringBuilder();
            string typeString = type.ToString();
            // Block
            code.AppendLine("Blockly.Blocks['comment" + typeString + "'] = {");
            code.AppendLine("init: function() {");
            code.AppendLine("        this.appendDummyInput()");
            code.AppendLine("            .appendField(new Blockly.FieldTextInput(\"comment\"), \"TEXT\");");
            if (type == 2)
            {
                code.AppendLine("        this.appendStatementInput(\"statement\")");
                code.AppendLine("            .setCheck(null);");
            }
            code.AppendLine("        this.setInputsInline(true);");
            code.AppendLine("        this.setPreviousStatement(true, null);");
            code.AppendLine("        this.setNextStatement(true, null);");
            code.AppendLine("        this.setColour(300);");
            code.AppendLine("        this.setTooltip('Comment block');");
            code.AppendLine("    }");
            code.AppendLine("};");

            // Code generator
            code.AppendLine("Blockly.CSharp['comment" + typeString + "'] = function(block) {");
            code.AppendLine("    var text_text = block.getFieldValue('TEXT');");            
            code.AppendLine("    var code = '//' + text_text + '\\n';");
            if (type == 2)
            {
                code.AppendLine("    var statements_statement = Blockly.CSharp.statementToCode(block, 'statement');");
                code.AppendLine("    code += statements_statement;");
            }
            code.AppendLine("    return code;");
            code.AppendLine("};");

            return code;
        }

        protected StringBuilder createMemberLoop()
        {
            StringBuilder code = new StringBuilder();
            // Block
            code.AppendLine("Blockly.Blocks['member_loop'] = {");
            code.AppendLine("init: function() {");
            code.AppendLine("        this.appendDummyInput()");
            code.AppendLine("            .appendField(\"Loop through members\");");
            code.AppendLine("        this.appendStatementInput(\"membercode\")");
            code.AppendLine("            .setCheck(null);");
            code.AppendLine("        this.setPreviousStatement(true, null);");
            code.AppendLine("        this.setNextStatement(true, null);");
            code.AppendLine("        this.setColour(120);");
            code.AppendLine("        this.setTooltip('Loop through the members of the same household as the current person');");
            code.AppendLine("    }");
            code.AppendLine("};");

            // Code generator
            code.AppendLine("Blockly.CSharp['member_loop'] = function(block) {");
            code.AppendLine("    var statements_membercode = Blockly.CSharp.statementToCode(block, 'membercode');");
            code.AppendLine("    var code = '';");
            code.AppendLine("    code += 'foreach(Person member in p.Household.Members.ToList())\\n';");
            code.AppendLine("    code += '{\\n';");
            code.AppendLine("    code += statements_membercode;");
            code.AppendLine("    code += '}\\n';");
            code.AppendLine("    return code;");
            code.AppendLine("};");

            return code;
        }

        protected StringBuilder createPersonProperties(string type, bool isSet)
        {
            StringBuilder code = new StringBuilder();

            // Block
            string householdFields = "";
            string name = type + "_properties" + (isSet ? "_set" : "_get");
            code.Append("Blockly.Blocks['" + name + "'] = {");
            code.AppendLine("init: function() {");
            code.AppendLine("        this.appendDummyInput()");
            code.AppendLine("            .appendField(\"" + (isSet ? "set " : "") + type + "\")");            
            code.Append("            .appendField(new Blockly.FieldDropdown([");
            if (type != "NewHousehold")
            {
                if (type != "Household")
                {
                    code.Append("[\"IsAlive\", \"IsAlive\"]");
                    code.Append(createFields(ModelSettings.Instance.PersonFields, ""));
                }
                if (ModelSettings.Instance.UseHouseholds && type != "Member")
                {
                    if (!isSet)
                    {
                        if (type == "Household")
                            code.Append("[\"MemberCount\", \"MemberCount\"]");
                        else
                            code.Append(", [\"Household.MemberCount\", \"Household.MemberCount\"]");
                    }
                    if (type != "Household")                    
                        householdFields = createFields(ModelSettings.Instance.HouseholdFields, "Household.");
                    else
                        householdFields = createFields(ModelSettings.Instance.HouseholdFields, "");
                }
            }
            else householdFields = createFields(ModelSettings.Instance.HouseholdFields, "").Substring(2);
            code.Append(householdFields);
            code.Append("]), \"" + name + "\")");           
            if (isSet) code.Append("            .appendField(\" to \")");
            code.AppendLine(";");
            if (isSet)
            {
                code.AppendLine("        this.setPreviousStatement(true, null);");
                code.AppendLine("        this.setNextStatement(true, null);");
                code.AppendLine("        this.appendValueInput(\"input\")");
                code.AppendLine("            .setCheck(null);");
            }
            else
            {
                code.AppendLine("        this.setOutput(true, null);");
            }
            code.AppendLine("        this.setInputsInline(true);");
            if (type != "Household")
                code.AppendLine("        this.setColour(120);");
            else
                code.AppendLine("        this.setColour(0);");
            code.AppendLine("        this.setTooltip('');");
            code.AppendLine("    }");
            code.AppendLine("};");

            // Code generator
            code.AppendLine("Blockly.CSharp['" + name + "'] = function(block) {");
            string preset = "";
            switch (type)
            {
                case "Member":
                    preset = "member";
                    break;
                default:
                    preset = type.ToLower()[0].ToString();
                    break;
            }
            if (isSet)
            {
                code.Append("    var code = '");
                if (type == "Member") code.Append("    lock(member) ");
                code.AppendLine(preset + ".' + this.getFieldValue('" + name + "') + ' = ' + (Blockly.CSharp.valueToCode(this, 'input') || 0.0) + ';\\n';");
                code.AppendLine("    return code;");
            }
            else
            {
                code.AppendLine("    var code = '" + preset + ".' + this.getFieldValue('" + name + "');");
                code.AppendLine("    return [code, Blockly.CSharp.ORDER_ATOMIC];");
            }
            code.AppendLine("};");
            return code;
        }

        private string createFields(List<ClassField> currentFields, string preface)
        {
            StringBuilder code = new StringBuilder();

            if (currentFields.Count != 0) code.Append(", ");            
            for (int i = 0; i < currentFields.Count; i++)
            {
                ClassField cf = currentFields[i];
                code.Append("[\"" + preface + cf.Name + "\",\"" + preface + cf.Name + "\"]");
                if (i != currentFields.Count - 1) code.Append(", ");
            }

            return code.ToString();
        }
    }
}
