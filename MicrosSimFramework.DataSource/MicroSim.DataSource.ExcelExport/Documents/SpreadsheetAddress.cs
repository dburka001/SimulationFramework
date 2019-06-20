using System;
using System.Text.RegularExpressions;
using Localizations = MicroSim.DataSource.ExcelExport.Properties.Resources;

namespace MicroSim.DataSource.ExcelExport.Documents
{
    public struct SpreadsheetAddress
    {
        private static readonly Regex AddressRegex = new Regex(@"^[A-Z]+[0-9]+$");
        private static readonly Regex ColumnPartRegex = new Regex(@"^[A-Z]+");
        private static readonly Regex RowPartRegex = new Regex(@"[0-9]+$");
        public static SpreadsheetAddress A1 => new SpreadsheetAddress("A", 1);
        public string Address { get; }

        public string Column { get; }

        public uint Row { get; }

        public SpreadsheetAddress(string column, uint row)
        {
            Row = row;
            Column = column.ToUpper();
            Address = $"{Column}{Row}";
        }

        public static uint ColumnToInt(string column)
        {
            if (string.IsNullOrWhiteSpace(column))
                throw new ArgumentNullException(nameof(column));

            column = column.ToUpperInvariant();
            var sum = 0u;

            for (var i = 0; i < column.Length; i++)
            {
                sum *= 26;
                sum += (uint)(column[i] - 'A' + 1);
            }

            return sum;
        }

        private static string IntToColumn(uint index)
        {
            var dividend = index;
            string column = string.Empty;
            uint modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                column = Convert.ToChar(65 + modulo).ToString() + column;
                dividend = (uint)((dividend - modulo) / 26);
            }

            return column;
        }

        public static SpreadsheetAddress Parse(string address)
        {
            address = address.ToUpperInvariant();

            if (!AddressRegex.IsMatch(address))
                throw new FormatException(
                    string.Format(Localizations.ExInvalidAddressFormat, address)
                    );

            var columnPartMatch = ColumnPartRegex.Match(address);
            var columnPart = address.Substring(columnPartMatch.Index, columnPartMatch.Length);

            var rowPartMatch = RowPartRegex.Match(address);
            var rowPart = address.Substring(rowPartMatch.Index, rowPartMatch.Length);

            return new SpreadsheetAddress(columnPart, uint.Parse(rowPart));
        }

        public SpreadsheetAddress NextColumn(uint offset = 1)
            => new SpreadsheetAddress(
                IntToColumn(ColumnToInt(Column) + offset),
                Row
                );

        public SpreadsheetAddress NextRow(uint offset = 1)
            => new SpreadsheetAddress(Column, Row + offset);
    }
}