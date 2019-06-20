using MicroSim.DataSource.Entities;
using System.Collections.Generic;
using System.Reflection;
using Localizations = MicroSim.DataSource.ExcelExport.Properties.Resources;

namespace MicroSim.DataSource.ExcelExport.Documents.Spreadsheets
{
    public class MsfMetadataWriter
    {
        private void WriteMetadata(
            OpenXmlSheet sheet,
            SpreadsheetAddress address,
            PropertyInfo meta
            )
        {
            sheet.WriteCell(address, meta.Name);
            address = address.NextColumn();
            var metaLabel = EntityUtil.GetPropertyCaption(meta);
            sheet.WriteCell(address, metaLabel);
        }

        public void Write(
            OpenXmlSheet sheet,
            SpreadsheetAddress address,
            PropertyInfo[] metadata
            )
        {
            sheet.WriteCell(address, Localizations.LblMetadataTitle);

            foreach (var meta in metadata)
            {
                address = address.NextRow();
                WriteMetadata(sheet, address, meta);
            }
        }
    }
}