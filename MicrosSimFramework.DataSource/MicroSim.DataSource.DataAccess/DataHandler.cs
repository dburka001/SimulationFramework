using MicroSim.DataSource.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

namespace MicroSim.DataSource.DataAccess
{
    /// <summary>
    /// Data Loader singleton
    /// </summary>
    public class DataHandler
    {
        /// <summary>
        /// The instance
        /// </summary>
        private static DataHandler _instance;        

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static DataHandler Instance
            => _instance ?? (_instance = new DataHandler());

        /// <summary>
        /// Prevents a default instance of the <see cref="DataHandler"/> class from being created.
        /// </summary>
        private DataHandler() { }

        /// <summary>
        /// Loads data from file.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath">The source.</param>
        /// <param name="hasHeader">if set to <c>true</c> [has header].</param>
        /// <returns></returns>
        public IEnumerable<T> LoadFromFile<T>(string filePath, bool hasHeader = true)
        {
            var fileType = DataFileTypeExtensions.Parse(Path.GetExtension(filePath));
            return LoadSv<T>(GetFullPath(filePath), fileType, hasHeader);
        }

        /// <summary>
        /// Loads T from a seperated value file.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="fileType">Type of the file.</param>
        /// <param name="hasHeader">if set to <c>true</c> [has header].</param>
        /// <returns></returns>
        private IEnumerable<T> LoadSv<T>(
            string source,
            DataFileType fileType,
            bool hasHeader)
        {                                                 
            var properties = typeof(T).GetProperties();
            var list = new List<T>();
            string geoFilter = typeof(IGeoEntity).IsAssignableFrom(typeof(T)) ? Settings.GeoFilter : "";            
            string delimiter = fileType.GetBaseDelimiter();            

            using (StreamReader sr = new StreamReader(source, Encoding.Default))
            {
                string[] header = null;
                if (hasHeader || fileType == DataFileType.TSV)
                    header = sr.ReadLine()
                        .Split(new string[] { delimiter }, StringSplitOptions.None);

                while (!sr.EndOfStream)
                {
                    string[] currentLine = sr.ReadLine()
                        .Split(new string[] { delimiter }, StringSplitOptions.None);                    

                    List<string[]> values = new List<string[]>();
                    switch (fileType)
                    {
                        case DataFileType.CSV: values.Add(currentLine); break;
                        case DataFileType.TSV:                            
                            string[] tsvProperties = currentLine[0].Split(',');                            
                            for (int i = 1; i < header.Length; i++)
                            {
                                var valueLine = new List<string>();
                                valueLine.AddRange(tsvProperties);
                                valueLine.Add(header[i]);
                                valueLine.Add(currentLine[i]);
                                values.Add(valueLine.ToArray());
                            }                            
                            break;
                        default: throw new NotImplementedException();
                    }

                    foreach (var line in values)
                    {
                        var newRecord = Activator.CreateInstance(typeof(T));

                        int pCounter = 0;
                        foreach (var p in properties)
                        {
                            var value = line[pCounter];

                            p.SetValue(newRecord, ValueParser.Instance.Parse(value, p.PropertyType));
                            pCounter++;
                        }

                        if (geoFilter != "" && 
                            ((IGeoEntity)newRecord).Geo != geoFilter)
                            continue;
                        list.Add((T)newRecord);
                    }                    
                }
            }

            return list;
        }

        /// <summary>
        /// Saves to file.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath">The file path.</param>
        /// <param name="data">The data.</param>
        public void SaveToFile<T>(string filePath, IEnumerable<T> data)
        {
            var output = new List<string>();
            var fileType = DataFileTypeExtensions.Parse(Path.GetExtension(filePath));
            string delimiter = fileType.GetBaseDelimiter();            
                        
            var properties = typeof(T).GetProperties();                
            var header = String.Join(delimiter, properties.Select(p => p.Name));
            output.Add(header);

            foreach (var d in data)
            {
                var line = new List<string>();
                foreach (var p in properties)
                {
                    var value = p.GetValue(d);
                    line.Add((value ?? "").ToString());
                }
                output.Add(String.Join(delimiter, line));
            }

            using (StreamWriter sw = new StreamWriter(GetFullPath(filePath), false, Encoding.UTF8))
                sw.Write(String.Join(Environment.NewLine, output));
        }

        /// <summary>
        /// Files the exists.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        public bool FileExists(string filePath)
        {
            return File.Exists(GetFullPath(filePath));
        }

        /// <summary>
        /// Gets the full path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        private string GetFullPath(string filePath)
        {
            return Path.Combine(Settings.WorkingDirectory, filePath);
        }

        /// <summary>
        /// Creates the specified directory.
        /// </summary>
        /// <param name="directoryName">Name of the directory.</param>
        public void CreateDirectory(string directoryName)
        {
            Directory.CreateDirectory(Path.Combine(Settings.WorkingDirectory, directoryName));
        }
    }
}
