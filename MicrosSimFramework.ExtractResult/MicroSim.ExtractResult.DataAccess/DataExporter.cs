using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.ExtractResource.DataAccess
{
    public static class DataExporter
    {
        public static event LogEventHandler Log = delegate { };

        public static void SaveResultData(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                Log(null, "Nem található a kiválaszatott mappa:\n\t" + folderPath);
                return;
            }

            Log(null, "Export: " + folderPath);
            foreach (var r in ResultData.Instance.Items)
            {
                var fileName = Path.Combine(folderPath, Path.ChangeExtension(r.Name, ".csv"));
                Log(null, "\t" + r.Name);
                using (StreamWriter sw = new StreamWriter(fileName, false, Encoding.UTF8))
                {
                    sw.Write(r.Data);
                }                
            }
            
            Log(null, "Sikeres mentés");
            Log(null, "");
        }
    }
}
