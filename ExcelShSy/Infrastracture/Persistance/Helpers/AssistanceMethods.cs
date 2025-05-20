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
        public static Dictionary<string, int>? GetRowValues(ExcelWorksheet worksheet, int fromRow, int toColumn)
        {
            var range = worksheet.Cells[fromRow, 1, fromRow, toColumn];
            bool HasEmpty = range.Any(cell => cell.Value != null);

#pragma warning disable CS8714, CS8621, CS8619
            if (HasEmpty)
            {
                var RowValues = range.Where(cell => !string.IsNullOrWhiteSpace(cell.Value?.ToString()))
                    .GroupBy(cell => cell.Value.ToString())
                    .ToDictionary(g => g.Key, g => g.First().Start.Column);
                return RowValues;
            }
#pragma warning restore CS8714, CS8621, CS8619

            return null;
        }
    }
}
