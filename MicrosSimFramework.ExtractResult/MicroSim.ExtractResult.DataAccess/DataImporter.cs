using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.ExtractResource.DataAccess
{
    public static class DataImporter
    {
        public static event LogEventHandler RunTimeRead = delegate { };
        public static event LogEventHandler Log = delegate { };

        public static void LoadResultData(string fileName)
        {
            if (!File.Exists(fileName))
            {
                Log(null, "Nem található a megadott fájl:\n\t" + fileName);
                return;
            }

            Log(null, "Import: " + fileName);

            ResultData.Instance.Items.Clear();

            using (StreamReader sr = new StreamReader(fileName, Encoding.Default))
            {
                string[] sor = sr.ReadLine().Split(';');
                RunTimeRead(null, "Futási idő: " + TimeSpan.FromSeconds(int.Parse(sor[1]) / 1000).ToString(@"hh\:mm\:ss"));

                List<string> resultText = new List<string>();
                ResultItem result = null;
                while (!sr.EndOfStream)
                {
                    sor = sr.ReadLine().Split(';');
                    if (sor.Length == 1)
                    {
                        if (result != null)
                            result.Data = String.Join(Environment.NewLine, resultText);
                        resultText.Clear();                        
                        result = new ResultItem() { Name = sor[0] };
                        Log(null, "\t" + result.Name);
                        ResultData.Instance.Items.Add(result);                        
                    }
                    else
                    {
                        resultText.Add(String.Join(";", sor).Replace('.', ','));
                    }
                }
                result.Data = String.Join(Environment.NewLine, resultText);
                
                Log(null, "Sikeres betöltés");
                Log(null, "");
            }
        }
    }
}
