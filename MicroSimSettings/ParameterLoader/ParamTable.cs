using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace MicroSimSettings
{
    [Serializable()]
    public class ParameterTable
    {
        public string Name { get; set; }
        public List<TáblázatSor> Táblázat = new List<TáblázatSor>(); //* pipa
        public List<string> OszlopNevek = new List<string>();   //* pipa
        public List<Nómenklatúra> nómenklatúrák = new List<Nómenklatúra>(); //*
        List<int> Eltolások;
        List<int> Elemszámok;
        public ParameterTable()
        {
            Táblázat = new List<TáblázatSor>();
            OszlopNevek = new List<string>();
            nómenklatúrák = new List<Nómenklatúra>();
            Eltolások = new List<int>();
            Elemszámok = new List<int>();
        }

        private string sourceCode;
        public string SourceCode
        {
            get
            {
                if (sourceCode == null) sourceCode = BuildCode().ToString();
                return sourceCode;
            }
        }

        public void AddLine(TáblázatSor sor)
        {
            Táblázat.Add(sor);
        }

        public class DinoComparer : IComparer<TáblázatSor>
        {
            int IComparer<TáblázatSor>.Compare(TáblázatSor x, TáblázatSor y)
            {
                short i = 0;
                while ((i < x.Indices.Count) && (x.Indices[i] == y.Indices[i])) i++;
                if (i == x.Indices.Count)
                    return 0;
                if (x.Indices[i] > y.Indices[i])
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            }
        }


        public void Proceess()
        {
            DinoComparer dc = new DinoComparer();
            Táblázat.Sort(dc);
            int dimenziószám = Táblázat[0].Indices.Count;
            for (int i = 0; i < dimenziószám; i++)
            {
                int smallest = (from x in Táblázat select x.Indices[i]).Min();
                int largest = (from x in Táblázat select x.Indices[i]).Max();
                Eltolások.Add(smallest);
                Elemszámok.Add(largest - smallest + 1);

            }
        }

        public StringBuilder BuildCode()
        {
            Proceess();

            StringBuilder sb = new StringBuilder();            
            
            List<int> dimensions = new List<int>();
            for (int i = 0; i < Táblázat[0].Indices.Count; i++)
            {
                dimensions = Táblázat[0].Indices;
            }
            StringBuilder SourceCodeText = new StringBuilder();

            //Build array with values
            SourceCodeText.Append("\t\tpublic static double[");//,,] " + TáblázatNév + "_array" + " = {");
            for (int i = 0; i < OszlopNevek.Count - 2; i++)
            {
                SourceCodeText.Append(",");
            }

            SourceCodeText.AppendLine("] " + Name + "_array" + " = {");
            SourceCodeText.Append("\t\t\t");
            for (int i = 0; i < OszlopNevek.Count - 2; i++)
            {
                SourceCodeText.Append("{");
            }
            SourceCodeText.Append(Táblázat[0].Value.ToString(CultureInfo.InvariantCulture));
            int sor = 1;
            while (sor < Táblázat.Count - 1)
            {
                while (Helpers.DimensionChange(dimensions, Táblázat[sor].Indices) == 0)
                {
                    SourceCodeText.Append(", " + Táblázat[sor].Value.ToString(CultureInfo.InvariantCulture));
                    if (sor == Táblázat.Count - 1) break;
                    else sor++;
                }
                for (int i = 0; i < Helpers.DimensionChange(dimensions, Táblázat[sor].Indices); i++)
                {
                    SourceCodeText.Append("}");
                }
                // SourceCodeText.Append(",");
                if (sor != Táblázat.Count - 1) SourceCodeText.Append(",");
                else SourceCodeText.Append("}");
                SourceCodeText.AppendLine();
                for (int i = 0; i < Helpers.DimensionChange(dimensions, Táblázat[sor].Indices); i++)
                {
                    if (i != 0) SourceCodeText.Append("{");
                    else SourceCodeText.Append("\t\t\t{");
                }
                if (sor != Táblázat.Count - 1) SourceCodeText.Append(Táblázat[sor].Value.ToString(CultureInfo.InvariantCulture));

                dimensions = Táblázat[sor].Indices;
                sor++;
            }

            SourceCodeText.Append("\t\t\t");
            for (int i = 0; i < OszlopNevek.Count - 2; i++)
            {
                SourceCodeText.Append("}");
            }
            SourceCodeText.Append(";");


            //Control tables
            SourceCodeText.AppendLine();
            string dimensionString = string.Join(",", dimensions.Select(n => n.ToString()).ToArray());
            string comaString = string.Join(",", dimensions.Select(n => "").ToArray());
                           
            //Building gunction argumnets list
            List<string> args = new List<string>();
            for (int i = 0; i < OszlopNevek.Count - 1; i++)
            {
                args.Add((nómenklatúrák[i] == null ? "int" : nómenklatúrák[i].listák[0]) + " " + Helpers.FirstCharToLower(OszlopNevek[i]));
            }

            //Building offsets
            StringBuilder offsets = new StringBuilder();
            for (int i = 0; i < OszlopNevek.Count - 1; i++)
            {
                string s = string.Format("int v{0}=", i.ToString());
                offsets.AppendLine("\t\t\t" + s + (nómenklatúrák[i] == null ? "" : "(int)") + Helpers.FirstCharToLower(OszlopNevek[i]) + "-" + Eltolások[i].ToString() + ";");
            }
            //Building params
            List<string> param = new List<string>(); ;
            for (int i = 0; i < OszlopNevek.Count - 1; i++)
            {
                param.Add("v" + i.ToString());
            }


            
            SourceCodeText.AppendLine();
            SourceCodeText.AppendLine();
            
            //Build Query Function
            StringBuilder f = new StringBuilder();
            f.AppendLine(string.Format("\t\tpublic static double {0}({1})", Name, Helpers.ListToComaSeparatedString(args)));
            f.AppendLine("\t\t{");                     
            f.AppendLine(offsets.ToString());            
            f.AppendLine("\t\t\treturn " + Name + "_array[" + Helpers.ListToComaSeparatedString(param) + "];");
            f.AppendLine("\t\t}");
            SourceCodeText.Append(f);

            SourceCodeText.AppendLine();
            SourceCodeText.AppendLine();
            return SourceCodeText;
        }
    }


}
