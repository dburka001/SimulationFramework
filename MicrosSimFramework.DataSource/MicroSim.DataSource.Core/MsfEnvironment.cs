using MicroSim.DataSource.Entities;
using System.IO;

namespace MicroSim.DataSource.Core
{
    public sealed class MsfEnvironment
    {
        private static string ExportDirectoryName = Resources.ExportDirectory;
        private static MsfEnvironment _instance;

        public static MsfEnvironment Current
            => _instance ?? (_instance = new MsfEnvironment());

        private MsfEnvironment()
        {
        }

        private static string GenerateFileName(string nameBase, int? idx, string extension)
            => $"{nameBase}{(idx.HasValue ? idx.Value.ToString() : string.Empty)}{extension}";

        private static string GetExportPath(
            bool createExportDirectory
            )
        {
            var path = Path.GetFullPath(
                Path.Combine(Settings.WorkingDirectory, ExportDirectoryName)
                );
            if (!Directory.Exists(path) && createExportDirectory)
                Directory.CreateDirectory(path);
            return path;
        }

        /// <summary>
        /// Gets the name of the export file.
        /// </summary>
        /// <param name="extension">The extension.</param>
        /// <returns>A file name for the export</returns>
        public string GetExportFileName(
            string fileNameBase,
            string extension,
            bool createExportDirectory = true
            )
        {
            if (!extension.StartsWith("."))
                extension = $".{extension}";

            var path = GetExportPath(
                createExportDirectory
                );

            int? idx = null;
            string fileName;
            while (File.Exists(fileName = Path.Combine(path, GenerateFileName(fileNameBase, idx, extension))))
                idx = idx.HasValue ? idx + 1 : 1;

            return fileName;
        }

        public string GetStaticExportFileName(
            string fileNameBase,
            string extension,
            bool createExportDirectory = true
            )
        {
            if (!extension.StartsWith("."))
                extension = $".{extension}";

            var path = GetExportPath(
                createExportDirectory
                );

            return Path.Combine(path, $"{fileNameBase}{extension}");
        }
    }
}