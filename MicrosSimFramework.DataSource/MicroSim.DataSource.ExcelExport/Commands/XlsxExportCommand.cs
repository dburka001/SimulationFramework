using MicroSim.DataSource.Entities;
using MicroSim.DataSource.ExcelExport.Documents;
using MicroSim.DataSource.ExcelExport.Documents.Spreadsheets;
using MicroSim.DataSource.Output;
using System;
using System.IO;

namespace MicroSim.DataSource.ExcelExport.Commands
{
    public class XlsxExportCommand : ExportCommandBase
    {
        private const string ParametersSheetName = "Parameters";
        private const string MetadataSheetName = "Metadata";
        private const string NomenclaturesSheetName = "Nomenclatures";

        private void WriteMetdataSheet(
            OpenXmlSheet sheet,
            Type metaDataBaseType
            )
        {
            var properties = metaDataBaseType.GetProperties();
            new MsfMetadataWriter()
                .Write(sheet, SpreadsheetAddress.A1, properties);
        }

        private void WritePartsSheet(
            OpenXmlSheet sheet,
            OutputDataSource datasource
            )
        {
            var partsWriter = new MsfDataSourcePartWriter();
            var address = SpreadsheetAddress.A1;

            foreach (var part in datasource.Parts)
            {
                if (part is DspOutputPopulation ||
                    part is DspOutputPopulationValidation) continue;

                var width = partsWriter.MeasureWidth(part);
                partsWriter.Write(sheet, part, address);

                address = address.NextColumn((uint)width + 1);
            }
        }

        private void WriteEnumsSheet(
            OpenXmlSheet sheet,
            Type[] types
            ) => new MsfEnumsWriter().Write(sheet, SpreadsheetAddress.A1, types);

        public override void Export(Stream target, OutputDataSource datasource, Type[] enumTypes)
        {
            var doc = OpenXmlUtil.CreateSpreadsheet(target);

            using (var sheet = OpenXmlSheet.Insert(doc, MetadataSheetName))
                WriteMetdataSheet(sheet, typeof(OutputPopulationEntity));

            using (var sheet = OpenXmlSheet.Insert(doc, NomenclaturesSheetName))
                WriteEnumsSheet(sheet, enumTypes);

            using (var sheet = OpenXmlSheet.Insert(doc, ParametersSheetName))
                WritePartsSheet(sheet, datasource);

            doc.WorkbookPart.Workbook.Save();
            doc.Save();
            doc.Close();
        }
    }
}