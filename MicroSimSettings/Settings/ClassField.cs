using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace MicroSimSettings
{
    public class ClassField
    {
        public DefaultType DefaultType { get; set; }
        public string Name { get; set; }
        public string DefaultValue { get; set; }        
        public DefaultField DefaultDataField { get; set; }

        public ClassField()
        {            
        }
        public ClassField(DefaultType type, string name)
        {
            SetValues(type, name, "", null);
        }
        public ClassField(DefaultType type, string name, string defaultValue)
        {
            SetValues(type, name, defaultValue, null);            
        }
        public ClassField(DefaultType type, string name, DefaultField defaultDataField)
        {
            SetValues(type, name, "", defaultDataField);
        }
        public ClassField(DefaultType type, string name, string defaultValue, DefaultField defaultDataField)
        {
            SetValues(type, name, defaultValue, defaultDataField);
        }

        private void SetValues(DefaultType type, string name, string defaultValue, DefaultField defaultDataField)
        {
            this.DefaultType = type;
            this.Name = name;
            this.DefaultValue = defaultValue;
            if (defaultDataField == null) defaultDataField = new DefaultField("", "");
            this.DefaultDataField = defaultDataField;
        }
    }

    public class DefaultType
    {
        public string Name { get; set; }
        public Type Type { get; set; }

        public DefaultType(string name, Type type)
        {
            Name = name;
            Type = type;
        }
    }

    public class DefaultField
    {
        public string ColumnName { get; set; }
        public string MetaName { get; set; }

        public DefaultField()
        {

        }

        public DefaultField(string name)
        {
            this.ColumnName = name;
        }

        public DefaultField(string name, string metaName)
        {
            this.ColumnName = name;
            this.MetaName = metaName;
        }
    }

    public class DefaultIdField
    {
        public int id { get; set; }
        public DefaultField Field { get; set; }

        public DefaultIdField()
        {
            id = 0;
            Field = null;
        }
    }
}
