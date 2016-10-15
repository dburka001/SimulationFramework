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
        private void createExtensionMethods()
        {
            mainCode += "\tpublic static class ExtensionMethods" + Environment.NewLine + "\t{" + Environment.NewLine;
            mainCode += ModelData.Instance.Parameters.BuildParamtableCode().ToString();
            createConstants();            
            createSimStep();
            if (ModelSettings.Instance.UseHouseholds)
            {
                createLeaveHousehold();
                createJoinHousehold();
                createJoinNewHousehold();
            }
            createGetResults();
            mainCode += "\t}" + Environment.NewLine;
        }

        private void createConstants()
        {
            foreach (Constant c in ModelSettings.Instance.Constants)
            {
                mainCode += "\t\tprivate static double " + c.Name + " = " + c.CurrentValue.ToString().Replace(',','.') + ";" + Environment.NewLine;
            }
            mainCode += Environment.NewLine;
        }

        private void createSimStep()
        {
            mainCode += "\t\tpublic static SimStepOutput SimStep";
            mainCode += "(int currentID, object currentPerson, int Year, int randomSeed)";
            mainCode += Environment.NewLine + "\t\t{" + Environment.NewLine;
            mainCode += "\t\t\tRandom rng = new Random(randomSeed);" + Environment.NewLine;
            mainCode += "\t\t\tModelSettings settings = ModelSettings.Instance;" + Environment.NewLine;
            mainCode += "\t\t\tPerson p = (Person)currentPerson;" + Environment.NewLine;
            mainCode += "\t\t\tSimStepOutput output = new SimStepOutput();" + Environment.NewLine;
            mainCode += "\t\t\tList<object> NewBorns = new List<object>();" + Environment.NewLine;
            mainCode += "\t\t\toutput.NewBorns = NewBorns;" + Environment.NewLine;
            mainCode += Environment.NewLine;
            mainCode += "\t\t\tif(!p.IsAlive) return output;" + Environment.NewLine;            
            SimStep();            
            mainCode += "\t\t\treturn output;" + Environment.NewLine;
            mainCode += "\t\t}" + Environment.NewLine;
            mainCode += Environment.NewLine;
        }

        private void SimStep()
        {
            // SimStep
            string simStepCode = (Environment.NewLine + ModelSettings.Instance.Code_SimStep).Replace("\n", "\n\t\t\t") + Environment.NewLine;
            string searchString = ""; int endIndex = -1;
            // Death
            searchString = "p.Die();";
            endIndex = simStepCode.IndexOf(searchString);
            if (endIndex != -1)
            {
                int startIndex = simStepCode.Substring(0, endIndex).LastIndexOf("\n");
                string indent = simStepCode.Substring(startIndex, endIndex - startIndex);
                string deathString = "";
                deathString += "p.Die();" + Environment.NewLine;
                deathString += indent + "return output;" + Environment.NewLine;
                simStepCode = simStepCode.Replace(searchString, deathString);
            }
            // Newborn
            searchString = "p.Born();";
            endIndex = simStepCode.IndexOf(searchString);
            if (endIndex != -1)
            {
                int startIndex = simStepCode.Substring(0, endIndex).LastIndexOf("\n");
                string indent = simStepCode.Substring(startIndex, endIndex - startIndex);
                string newBornString = "";
                newBornString += "Person n = new Person();" + Environment.NewLine;
                newBornString += indent + "Person m = p;" + Environment.NewLine; // Create variable for mother                
                if (ModelSettings.Instance.UseHouseholds)
                {
                    newBornString += indent + "n.Household = p.Household;" + Environment.NewLine; // Set newborn Household
                    newBornString += indent + "n.Household.Members.Add(n);" + Environment.NewLine; 
                }
                newBornString += indent + "n.ParentID = currentID.ToString() + NewBorns.Count.ToString();" + Environment.NewLine; // Create variable for mother                                
                newBornString += (Environment.NewLine + ModelSettings.Instance.Code_NewBorn).Replace("\n", indent);
                newBornString += indent + "NewBorns.Add(n);" + Environment.NewLine;
                simStepCode = simStepCode.Replace(searchString, newBornString);
            }
            // Leave Household
            searchString = "p.LeaveHousehold();";
            endIndex = simStepCode.IndexOf(searchString);
            if (endIndex != -1)
            {
                int startIndex = simStepCode.Substring(0, endIndex).LastIndexOf("\n");
                string indent = simStepCode.Substring(startIndex, endIndex - startIndex);
                string leaveHouseholdString = "";
                leaveHouseholdString += "output.NewHousehold = new Household();" + Environment.NewLine;
                leaveHouseholdString += indent + "((Household)output.NewHousehold).ParentID = currentID.ToString();" + Environment.NewLine;
                leaveHouseholdString += indent + "ExtensionMethods.JoinNewHousehold(p, output.NewHousehold, \"Single\");" + Environment.NewLine;                
                simStepCode = simStepCode.Replace(searchString, leaveHouseholdString);
            }
            // Add code
            mainCode += simStepCode;
        }

        private void createLeaveHousehold()
        {
            mainCode += "\t\tpublic static void LeaveHousehold(object person)";
            mainCode += Environment.NewLine + "\t\t{" + Environment.NewLine;
            mainCode += "\t\t\tPerson p = (Person)person;" + Environment.NewLine;
            mainCode += "\t\t\tHousehold oldHousehold = p.Household;" + Environment.NewLine;
            mainCode += "\t\t\tlock(oldHousehold.Members) {" + Environment.NewLine;
            mainCode += "\t\t\t\toldHousehold.Members.Remove(p);" + Environment.NewLine;
            mainCode += "\t\t\t\tp.Household = null;" + Environment.NewLine;
            mainCode += "\t\t\t\tif(oldHousehold.Members.Count == 0) oldHousehold.IsEmpty = true;" + Environment.NewLine;
            mainCode += "\t\t\t}" + Environment.NewLine;
            mainCode += "\t\t}" + Environment.NewLine;
            mainCode += Environment.NewLine;
        }

        private void createJoinHousehold()
        {
            mainCode += "\t\tpublic static void JoinHousehold(object person, object newHousehold)";
            mainCode += Environment.NewLine + "\t\t{" + Environment.NewLine;
            mainCode += "\t\t\tPerson p = (Person)person;" + Environment.NewLine;
            mainCode += "\t\t\tExtensionMethods.LeaveHousehold(p);" + Environment.NewLine;            
            mainCode += "\t\t\tp.Household = (Household)newHousehold;" + Environment.NewLine;
            mainCode += "\t\t\t((Household)newHousehold).Members.Add(p);" + Environment.NewLine;            
            mainCode += "\t\t}" + Environment.NewLine;
            mainCode += Environment.NewLine;
        }

        private void createJoinNewHousehold()
        {
            mainCode += "\t\tpublic static void JoinNewHousehold(object person, object newHousehold, string joinType)";
            mainCode += Environment.NewLine + "\t\t{" + Environment.NewLine;
            mainCode += "\t\t\tPerson p = (Person)person;" + Environment.NewLine;           
            mainCode += "\t\t\tHousehold oldHousehold = p.Household;" + Environment.NewLine;                                    
            mainCode += "\t\t\tHousehold n = (Household)newHousehold;" + Environment.NewLine;
            mainCode += (Environment.NewLine + ModelSettings.Instance.Code_HouseholdJoinNew).Replace("\n", "\n\t\t\t") + Environment.NewLine;
            mainCode += "\t\t\tExtensionMethods.LeaveHousehold(p);" + Environment.NewLine;
            mainCode += "\t\t\tp.Household = n;" + Environment.NewLine;
            mainCode += "\t\t\tn.Members.Add(p);" + Environment.NewLine;
            mainCode += "\t\t}" + Environment.NewLine;
            mainCode += Environment.NewLine;
        }

        private void createGetResults()
        {
            mainCode += "\t\tpublic static void GetResults(object inputPopulation, List<object> inputHouseholds, List<Result> ResultList, int Year)";
            mainCode += Environment.NewLine + "\t\t{" + Environment.NewLine;
            mainCode += "\t\t\tList<Person> Population = (List<Person>)inputPopulation;" + Environment.NewLine;            
            if(ModelSettings.Instance.UseHouseholds)
                mainCode += "\t\t\tList<Household> Households = inputHouseholds.Cast<Household>().ToList();" + Environment.NewLine;
            mainCode += (Environment.NewLine + ModelSettings.Instance.Code_ResultSettings).Replace("\n", "\n\t\t\t") + Environment.NewLine;                        
            mainCode += "\t\t}" + Environment.NewLine;
        }
    }
}
