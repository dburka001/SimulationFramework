using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MicroSimSettings
{
    [Serializable()]
    public class SimulationEnvironment
    {
        public BindingList<Nómenklatúra> Nómenklatúrák = new BindingList<Nómenklatúra>();
        public BindingList<ParameterTable> ValószínűségiTáblák = new BindingList<ParameterTable>();
        public Dictionary<string, string> MetaDictionary = new Dictionary<string, string>();       // TODO  

        public StringBuilder BuildNomenclatureCode()
        {
            StringBuilder SourceCodeText = new StringBuilder();            
            foreach (Nómenklatúra nómenklatúra in Nómenklatúrák)
            {
                SourceCodeText.Append(nómenklatúra.BuildCode());
                SourceCodeText.AppendLine();
            }
            return SourceCodeText;
        }

        public StringBuilder BuildParamtableCode()
        {
            StringBuilder SourceCodeText = new StringBuilder();
            SourceCodeText.AppendLine();
            foreach (ParameterTable valószínűségitábla in ValószínűségiTáblák)
            {
                SourceCodeText.Append(valószínűségitábla.BuildCode());
            }
            SourceCodeText.AppendLine();
            return SourceCodeText;
        }
    }
}
