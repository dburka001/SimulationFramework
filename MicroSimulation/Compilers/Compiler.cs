using MicroSimSettings;
using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MicroSimulation
{
    public partial class Compiler
    {
        // Private        
        private CSharpCodeProvider cscProvider = new CSharpCodeProvider();
        private CompilerParameters parameters = new CompilerParameters();
        private string mainCode = "";

        public string Name { get; set; }


        public Compiler()
        {
            this.Name = "CompiledNamspace" + String.Format("Foo_{0}", Guid.NewGuid().ToString("N"));
            this.AddField(DynamicClassType.Person, new ClassField(ModelSettings.Instance.DefaultTypes[1], "IsAlive", "true"));
            this.AddField(DynamicClassType.Person, new ClassField(ModelSettings.Instance.DefaultTypes[0], "ParentID", ""));
            this.AddField(DynamicClassType.Household, new ClassField(ModelSettings.Instance.DefaultTypes[1], "IsEmpty", "false"));
            this.AddField(DynamicClassType.Household, new ClassField(ModelSettings.Instance.DefaultTypes[0], "ParentID", ""));            
        }
                
        private void createMainCode()
        {
            mainCode = "";
            // Using
            mainCode += "using System;" + Environment.NewLine;           
            mainCode += "using System.Collections;" + Environment.NewLine;
            mainCode += "using System.Collections.Generic;" + Environment.NewLine;
            mainCode += "using MicroSimSettings;" + Environment.NewLine;            
            mainCode += "using System.Linq;" + Environment.NewLine;
            mainCode += "using Microsoft.Win32.SafeHandles;" + Environment.NewLine;
            mainCode += "using System.Collections.Concurrent;" + Environment.NewLine;
            mainCode += "using System.Threading.Tasks;" + Environment.NewLine;            
            mainCode += Environment.NewLine;
            mainCode += "namespace " + this.Name + Environment.NewLine + "{" + Environment.NewLine;
            mainCode += ModelData.Instance.Parameters.BuildNomenclatureCode().ToString();                      
            createDynamicClass(DynamicClassType.Person); mainCode += Environment.NewLine;
            if (ModelSettings.Instance.UseHouseholds)
            { createDynamicClass(DynamicClassType.Household); mainCode += Environment.NewLine; }
            createExtensionMethods();
            if(ModelSettings.Instance.UseHouseholds) createRelationShipExtensions();
            mainCode += "}";
        }        
               
        public CompilerResults Compile()
        {
            parameters.GenerateInMemory = true;
            parameters.GenerateExecutable = false;
            parameters.ReferencedAssemblies.Add("System.dll");
            parameters.ReferencedAssemblies.Add("System.Core.dll");            
            parameters.ReferencedAssemblies.Add("Microsoft.CSharp.dll");            
            parameters.ReferencedAssemblies.Add("MicroSimSettings.dll");
            parameters.ReferencedAssemblies.Add("MicroSimulation.dll");
            createMainCode();
            CompilerResults result = cscProvider.CompileAssemblyFromSource(parameters, mainCode);
            createLog(result);
            return result;
        }

        private void createLog(CompilerResults result)
        {
            StreamWriter sw = new StreamWriter("Compile.log", false, Encoding.Default);
            try
            {
                List<CompilerError> errorList = result.Errors.Cast<CompilerError>().ToList();
                if (errorList.Count > 0)
                {
                    sw.WriteLine("ERRORS" + Environment.NewLine);
                    MessageBox.Show("Compiler Error in " + this.Name); //TODO exception
                    errorList.ForEach(error => sw.WriteLine(error.Line + ": " + error.ErrorText));
                    sw.WriteLine(Environment.NewLine + new String('-', 40) + Environment.NewLine);
                }
                sw.WriteLine(mainCode);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to create " + "Compile.log" + Environment.NewLine + ex.Message);
            }
            finally
            {
                sw.Close();
            }
        }
    }
}
