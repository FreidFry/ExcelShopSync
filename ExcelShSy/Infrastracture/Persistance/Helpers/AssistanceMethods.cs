using ExcelShSy.Core.Interfaces.Excel;
using OfficeOpenXml;

namespace ExcelShSy.Infrastracture.Persistance.Helpers
{
    public class AssistanceMethods : IAssistanceMethods
    {
        public Dictionary<string, int>? GetRowValues(ExcelWorksheet worksheet, int fromRow, int toColumn)
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
