using MicroSim.DataSource.Core;
using MicroSim.DataSource.Entities;
using System.Linq;
using System.Reflection;
using System.Windows;
using Localizations = MicroSim.DataSource.ExcelExport.Properties.Resources;

namespace MicroSim.DataSource.ExcelExport.Documents.Spreadsheets
{
    public class MsfDataSourcePartWriter
    {
        private PropertyInfo[] GetProperties(IMsfExportableDataSource part)
            => part.InputPartType
                .GetProperties()
                .Where(p => p.CanRead && p.CanWrite)
                .ToArray();

        private void WriteHeader(
            OpenXmlSheet sheet,
            PropertyInfo[] properties,
            string title,
            SpreadsheetAddress address
            )
        {
            sheet.WriteCell(
                address.Row,
                address.Column,
                string.Format(
                    Localizations.LblPartsTableTitleFormat, 
                    OpenXmlUtil.NormalizeText(title))
                );
            address = address.NextRow();

            foreach (var prop in properties)
            {
                sheet.WriteCell(
                    address.Row,
                    address.Column,
                    OpenXmlUtil.NormalizeText(EntityUtil.GetPropertyCaption(prop))
                    );

                address = address.NextColumn();
            }
        }        

        private void WriteRow(
            OpenXmlSheet sheet,
            PropertyInfo[] properties,
            object item,
            SpreadsheetAddress address
            )
        {
            var row = sheet.GetRow(address.Row);
            foreach (var prop in properties)
            {
                row.WriteCell(
                    address.Column,
                    prop.GetValue(item)
                    );
                address = address.NextColumn();
            }
        }

        public int MeasureWidth(IMsfExportableDataSource part)
            => GetProperties(part).Length;

        public Size Measure(
            IMsfExportableDataSource part
            ) => new Size(
                MeasureWidth(part),
                part.GetData().Count()
                );

        public void Write(
            OpenXmlSheet sheet,
            IMsfExportableDataSource part,
            SpreadsheetAddress address
            )
        {
            var properties = GetProperties(part);
            WriteHeader(
                sheet,
                properties,
                part.Title,
                address
                );
            address = address.NextRow(2);

            foreach (var item in part.GetData())
            {
                WriteRow(sheet, properties, item, address);
                address = address.NextRow();
            }
        }
    }
}