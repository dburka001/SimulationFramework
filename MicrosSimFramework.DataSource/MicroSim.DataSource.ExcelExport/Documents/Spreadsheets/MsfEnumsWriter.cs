using MicroSim.DataSource.Entities;
using System;
using Localizations = MicroSim.DataSource.ExcelExport.Properties.Resources;


namespace MicroSim.DataSource.ExcelExport.Documents.Spreadsheets
{
    public class MsfEnumsWriter
    {
        private void WriteEnum(
            OpenXmlSheet sheet,
            Type enumType,
            ref SpreadsheetAddress address
            )
        {
            var underlayingType = Enum.GetUnderlyingType(enumType);
            sheet.WriteCell(address, 
                string.Format(
                    Localizations.LblListTitleFormat, 
                    OpenXmlUtil.NormalizeText(EnumExtensions.GetDisplayName(enumType))));

            foreach (Enum value in enumType.GetEnumValues())
            {
                if (value.IsHidden()) continue;

                var caddr = address = address.NextRow();
                var name = value.GetLabel();
                var numeric = Convert.ChangeType(value, underlayingType);

                sheet.WriteCell(caddr, name);
                caddr = caddr.NextColumn();
                sheet.WriteCell(caddr, numeric);
            }
        }

        public void Write(
            OpenXmlSheet sheet,
            SpreadsheetAddress address,
            Type[] enums
            )
        {
            foreach (var enumType in enums)
            {
                WriteEnum(sheet, enumType, ref address);
                address = address.NextRow(2);
            }
        }
    }
}