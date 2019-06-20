using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Linq;

namespace MicroSim.DataSource.ExcelExport.Documents
{
    public class OpenXmlRow
    {
        internal OpenXmlRow(WorksheetPart sheetPart, Sheet sheet, Row row)
        {
            Row = row ?? throw new ArgumentNullException(nameof(row));
            SheetPart = sheetPart ?? throw new ArgumentNullException(nameof(sheetPart));
            Sheet = sheet ?? throw new ArgumentNullException(nameof(sheet));
        }

        public Row Row { get; }
        public uint RowIndex => Row.RowIndex.Value;
        public Sheet Sheet { get; }

        public WorksheetPart SheetPart { get; }

        public void WriteCell(string column, object value)
        {
            var cellref = column + RowIndex.ToString();
            var sheetData = SheetPart.Worksheet.GetFirstChild<SheetData>();

            var cell = Row.Elements<Cell>()
                .FirstOrDefault(c => c.CellReference.Value == cellref)
               ?? InsertCell(cellref, Row);

            OpenXmlUtil.SetCellValue(cell, value);
        }

        private Cell InsertCell(string cellref, Row row)
        {
            Cell refCell = null;
            foreach (var c in row.Elements<Cell>())
            {                
                if (OpenXmlUtil.CompareCellColumnRef(cellref, c.CellReference.Value) < 0)
                {
                    refCell = c;
                    break;
                }
            }

            var newCell = new Cell() { CellReference = cellref };
            row.InsertBefore(newCell, refCell);

            return newCell;
        }
    }
}