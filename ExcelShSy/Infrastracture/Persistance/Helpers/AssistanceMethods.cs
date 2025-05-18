using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelShSy.Infrastracture.Persistance.Helpers
{
    public static class AssistanceMethods
    {
        public static List<string> GetRowValues(ExcelWorksheet worksheet, int fromRow, int fromColumn, int toColumn)
        {
            var values = worksheet.Cells[fromRow, fromColumn, fromRow, toColumn];
            bool HasEmpty = values.Any(cell => cell.Value != null);

            if (HasEmpty)
            {
                var RowValues = values.Where(cell => cell.Value != null).Select(cell => cell.Value.ToString()).ToList();
                return RowValues;
            }

            return [];
        }
    }
}
