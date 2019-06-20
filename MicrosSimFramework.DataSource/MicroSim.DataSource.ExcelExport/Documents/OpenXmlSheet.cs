using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Localizations = MicroSim.DataSource.ExcelExport.Properties.Resources;

namespace MicroSim.DataSource.ExcelExport.Documents
{
    public class OpenXmlSheet : IDisposable
    {
        private readonly Hashtable _rowIndices = new Hashtable();

        public OpenXmlSheet(WorkbookPart workbook, string sheetName)
        {
            Sheet = workbook.Workbook
                .Descendants<Sheet>().Where(s => string.Equals(s.Name, sheetName, StringComparison.CurrentCultureIgnoreCase))
                .FirstOrDefault();

            CheckSheet(sheetName);

            SheetPart = (WorksheetPart)workbook.GetPartById(this.Sheet.Id);

            BuildRowIndices();
        }

        public Sheet Sheet { get; }

        public WorksheetPart SheetPart { get; }

        public Cell this[string address]
            => SheetPart.Worksheet
                .Descendants<Cell>()
                .Where(c => c.CellReference == address)
                .FirstOrDefault();

        public Cell this[uint row, string column]
            => this[column + row.ToString()];

        public static OpenXmlSheet Insert(SpreadsheetDocument doc, string sheetName)
        {
            OpenXmlUtil.InsertSheet(doc, sheetName);
            return new OpenXmlSheet(doc.WorkbookPart, sheetName);
        }

        public OpenXmlRow GetRow(uint rowIndex)
        {
            var sheetData = SheetPart.Worksheet.GetFirstChild<SheetData>();
            var row = FindRowByNumber(sheetData, rowIndex);

            return new OpenXmlRow(SheetPart, Sheet, row);
        }

        public void WriteCell(SpreadsheetAddress address, object value)
            => WriteCell(address.Row, address.Column, value);

        public void WriteCell(uint rowIndex, string column, object value)
        {
            var cellref = column + rowIndex.ToString();
            var sheetData = SheetPart.Worksheet.GetFirstChild<SheetData>();

            var row = FindRowByNumber(sheetData, rowIndex);

            var cell = row.Elements<Cell>()
                .FirstOrDefault(c => c.CellReference.Value == cellref)
               ?? InsertCell(cellref, row);

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

        private void BuildRowIndices()
        {
            var sheetData = SheetPart.Worksheet.GetFirstChild<SheetData>();

            foreach (var row in sheetData.OfType<Row>())
                _rowIndices.Add(row.RowIndex.Value, row);
        }

        private void CheckSheet(string sheetName)
        {
            if (Sheet == null)
                throw new ExcelException(
                    string.Format(Localizations.ExSheetIsMissing, sheetName)
                    );
        }

        private Row FindRowByNumber(SheetData sheetData, uint rowIndex)
        {
            return _rowIndices.ContainsKey(rowIndex)
                ? (Row)_rowIndices[rowIndex]
                : InsertRow(rowIndex, sheetData);
        }

        private Row InsertRow(uint rowIndex, SheetData sheetData)
        {
            Row row = new Row() { RowIndex = rowIndex };
            sheetData.Append(row);
            _rowIndices.Add(row.RowIndex.Value, row);

            return row;
        }

        #region IDisposable Support
        private bool _isDispopsed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDispopsed)
            {
                if (disposing)
                {
                    _rowIndices.Clear();
                }

                _isDispopsed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion

    }
}