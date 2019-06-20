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
    public enum DynamicClassType { Person, Household }

    public partial class Compiler
    {
        // Private
        private List<ClassField> fieldsOfPerson = new List<ClassField>();
        private List<ClassField> fieldsOfHousehold = new List<ClassField>();

        public void AddField(DynamicClassType type, ClassField field)
        {
            switch (type)
            {
                case DynamicClassType.Person:
                    fieldsOfPerson.Add(field);
                    break;
                case DynamicClassType.Household:
                    fieldsOfHousehold.Add(field);
                    break;
                default:
                    break;
            }
        }

        private string currentClassName = "";

        private void createDynamicClass(DynamicClassType type)
        {
            List<ClassField> currentFields = null;
            switch (type)
            {
                case DynamicClassType.Person:
                    currentClassName = "Person";
                    currentFields = fieldsOfPerson;
                    break;
                case DynamicClassType.Household:
                    currentClassName = "Household";
                    currentFields = fieldsOfHousehold;
                    break;
                default:
                    break;
            }
            mainCode += "\tpublic class " + currentClassName + Environment.NewLine + "\t{" + Environment.NewLine;
            mainCode += createFields(type, currentFields);
            mainCode += createConstructor(type, currentFields);
            mainCode += createDieMethod(type);
            mainCode += createCloneMethod(type, currentFields);
            mainCode += "\t}" + Environment.NewLine;
        }

        private string createFields(DynamicClassType type, List<ClassField> currentFields)
        {
            string fieldCode = "";
            if (type == DynamicClassType.Household)
            {
                fieldCode += "\t\tpublic List<Person> Members { get; set; }" + Environment.NewLine;
                //fieldCode += "\t\tpublic int MemberCount { get { return Members.Where( p => p.FamilyStatus == 4 ).Count(); } }" + Environment.NewLine;
                fieldCode += "\t\tpublic int MemberCount { get { return Members.Count(); } }" + Environment.NewLine;
            }
            if (type == DynamicClassType.Person && ModelSettings.Instance.UseHouseholds)
                fieldCode += "\t\tpublic Household Household { get; set; }" + Environment.NewLine;
            foreach (ClassField field in currentFields)
            {
                fieldCode += "\t\tpublic " + field.DefaultType.Type.ToString() + " " + field.Name + " { get; set; }" + Environment.NewLine;
            }
            fieldCode += Environment.NewLine;
            return fieldCode;
        }

        private string createConstructor(DynamicClassType type, List<ClassField> currentFields)
        {
            string constructorCode = "";
            constructorCode += "\t\tpublic " + currentClassName + "()" + Environment.NewLine + "\t\t{" + Environment.NewLine;
            if (type == DynamicClassType.Household) constructorCode += "\t\t\tthis.Members = new List<Person>();" + Environment.NewLine;
            foreach (ClassField field in currentFields.Where(x => x.DefaultValue != ""))
            {
                constructorCode += "\t\t\tthis." + field.Name + "=";
                if (field.DefaultType.Type == typeof(string)) constructorCode += "\"" + field.DefaultValue + "\"";
                else constructorCode += field.DefaultValue;
                constructorCode += ";" + Environment.NewLine;
            }
            constructorCode += "\t\t}" + Environment.NewLine + Environment.NewLine;

            return constructorCode;
        }

        private string createDieMethod(DynamicClassType type)
        {
            string dieCode = "";
            if (type == DynamicClassType.Person)
            {
                dieCode += "\t\tpublic void Die()" + Environment.NewLine + "\t\t{" + Environment.NewLine;
                dieCode += "\t\t\tIsAlive = false;" + Environment.NewLine;
                if(ModelSettings.Instance.UseHouseholds)
                dieCode += "\t\t\tExtensionMethods.LeaveHousehold(this);" + Environment.NewLine;
                dieCode += "\t\t}" + Environment.NewLine + Environment.NewLine;
            }

            return dieCode;
        }

        private string createCloneMethod(DynamicClassType type, List<ClassField> currentFields)
        {
            string cloneCode = "";
            cloneCode += "\t\tpublic " + currentClassName + " Clone()" + Environment.NewLine;
            cloneCode += "\t\t{" + Environment.NewLine;
            cloneCode += "\t\t\t" + currentClassName + " " + currentClassName.Substring(0,1).ToLower() + " = new " + currentClassName + "();" + Environment.NewLine;     
            foreach (ClassField field in currentFields)
            {
                cloneCode += "\t\t\t" + currentClassName.Substring(0, 1).ToLower() + "." + field.Name + " = this." + field.Name + ";" + Environment.NewLine;
            }
            cloneCode += "\t\t\treturn " + currentClassName.Substring(0, 1).ToLower() + ";" + Environment.NewLine;
            cloneCode += "\t\t}" + Environment.NewLine;
            return cloneCode;
        }        
    }
}
