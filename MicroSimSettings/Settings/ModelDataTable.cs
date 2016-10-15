using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSimSettings
{
    public class ModelDataTable : DataTable
    {
        public string SourceFileName { get; set; }        
        public string Description { get; set; }

        public ModelDataTable(string source)
        {
            LoadData(source);
        }

        public void LoadData(string source)
        {
            this.SourceFileName = source;

            StreamReader sr = new StreamReader(SourceFileName, Encoding.Default);

            string[] header = sr.ReadLine().Split(',', ';');
            foreach (string item in header)
            {
                this.Columns.Add(item, typeof(string));
            }

            while (!sr.EndOfStream)
            {
                string[] rowData = sr.ReadLine().Split(',', ';');
                this.Rows.Add(rowData);
            }

            /*
            DataColumn emptyCol = new DataColumn(" ");
            this.Columns.Add(emptyCol);
            emptyCol.SetOrdinal(0);
            */

            sr.Close();
        }
    }
}
