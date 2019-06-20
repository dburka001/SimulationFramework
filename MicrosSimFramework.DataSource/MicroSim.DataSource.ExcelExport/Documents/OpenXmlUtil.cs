using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Globalization;
using System.IO;
using System.Linq;

namespace MicroSim.DataSource.ExcelExport.Documents
{

    public static class OpenXmlUtil
    {
        private static readonly CultureInfo XlsxExportCulture = new CultureInfo("en-US");

        internal static bool IsNumeric(Type valueType)
            => valueType == null
                ? false
                : new[]
                    {
                        TypeCode.Byte,
                        TypeCode.SByte,
                        TypeCode.UInt16,
                        TypeCode.UInt32,
                        TypeCode.UInt64,
                        TypeCode.Int16,
                        TypeCode.Int32,
                        TypeCode.Int64,
                        TypeCode.Decimal,
                        TypeCode.Double,
                        TypeCode.Single,
            }.Contains(Type.GetTypeCode(valueType));


        private static string ToCellText(object value, CultureInfo exportCulture)
        {
            if (value == null)
                return string.Empty;

            var valueType = value.GetType();
            if (valueType.IsEnum)
                return Convert.ChangeType(value, typeof(int)).ToString();

            return value as string
                ?? Convert.ToString(value, exportCulture);
        }

        public static void SetCellValue(
            Cell cell,
            object value
            )
        {
            cell.DataType = new EnumValue<CellValues>(
                IsNumeric(value?.GetType())
                    ? CellValues.Number
                    : CellValues.String
                );
            cell.CellValue = new CellValue(
                ToCellText(value, XlsxExportCulture)
                );

        }

        public static SpreadsheetDocument CreateSpreadsheet(Stream stream)
        {
            var doc = SpreadsheetDocument.Create(
                stream,
                SpreadsheetDocumentType.Workbook
                );
            var workbookPart = doc.AddWorkbookPart();
            workbookPart.Workbook = new Workbook();
            workbookPart.Workbook.AppendChild(new Sheets());

            return doc;
        }

        public static Sheet InsertSheet(SpreadsheetDocument doc, string sheetName)
        {
            var newWorksheetPart =
                doc.WorkbookPart.AddNewPart<WorksheetPart>();
            newWorksheetPart.Worksheet = new Worksheet(new SheetData());

            var sheets = doc.WorkbookPart.Workbook.GetFirstChild<Sheets>();
            var relationshipId = doc.WorkbookPart
                .GetIdOfPart(newWorksheetPart);
            var sheetID = sheets.Elements<Sheet>()
                .Select(s => s.SheetId.Value)
                .DefaultIfEmpty(0u)
                .Max() + 1;

            var sheet = new Sheet()
            {
                Id = relationshipId,
                SheetId = sheetID,
                Name = sheetName
            };
            sheets.Append(sheet);

            return sheet;
        }

        public static int CompareCellColumnRef(string cellRef1, string cellRef2)
        {
            var cell1 = SpreadsheetAddress.Parse(cellRef1);
            var cell2 = SpreadsheetAddress.Parse(cellRef2);
            var c1 = SpreadsheetAddress.ColumnToInt(cell1.Column);
            var c2 = SpreadsheetAddress.ColumnToInt(cell2.Column);

            return (int)(c1 - c2);           
        }

        public static string NormalizeText(string text)
        {
            var newTitle = text.ToCharArray();
            newTitle[0] = char.ToUpper(newTitle[0]);
            for (int i = 1; i < newTitle.Length; i++)
            {
                if (newTitle[i - 1] == ' ')
                    newTitle[i] = char.ToUpper(newTitle[i]);
            }
            return new string(newTitle).Replace(" ", string.Empty);
        }
    }
}