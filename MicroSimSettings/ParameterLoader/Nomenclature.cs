using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MicroSimSettings
{
    [Serializable()]
    public class NómentklatúraElem
    {
        public NómentklatúraElem(string originalName, int value)
        {
            this.OriginalName = originalName;
            this.Value = value;
            this.CodeName = Helpers.StringHelper(originalName);
        }

        public NómentklatúraElem(string originalName)
        {
            this.OriginalName = originalName;
            this.CodeName = Helpers.StringHelper(originalName);
        }

        public string OriginalName { get; set; }
        public string CodeName { get; set; }
        public int Value { get; set; }
    }

    [Serializable()]
    public class Nómenklatúra
    {
        public string Name { get; set; }
        public string MetaName { get; set; }

        public BindingList<NómentklatúraElem> Elemek = new BindingList<NómentklatúraElem>();

        public List<string> listák;

        public string GetElementNameByID(int ID)
        {
            var name = (from x in Elemek where x.Value == ID select x.OriginalName).FirstOrDefault();
            if (name == null)
            {
                //ToDo: error handling
                return "<Error>";
            }
            else
                return name;
        }

        private StringBuilder sourceCode;
        public StringBuilder SourceCode
        {
            get
            {
                if (sourceCode == null) sourceCode = BuildCode();
                return sourceCode;
            }
        }

        public StringBuilder BuildCode()
        {
            StringBuilder SourceCodeText = new StringBuilder();
            for (int i = 0; i < listák.Count; i++)
            {
                string s = listák[i];
                listák[i] = Helpers.StringHelper(s);
                if (i == 0)
                {
                    SourceCodeText.AppendLine("\tpublic enum " + listák[i] + " {");
                }
                else if (i == listák.Count - 1)
                {
                    SourceCodeText.Append("\t\t" + listák[i] + " }");
                    SourceCodeText.AppendLine();
                }
                else
                {
                    SourceCodeText.AppendLine("\t\t" + listák[i] + ",");
                }
            }
            return SourceCodeText;
        }
    }
}
