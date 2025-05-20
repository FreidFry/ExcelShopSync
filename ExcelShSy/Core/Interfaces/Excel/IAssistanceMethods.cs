using OfficeOpenXml;

namespace ExcelShSy.Core.Interfaces.Excel
{
    public interface IAssistanceMethods
    {
        Dictionary<string, int>? GetRowValues(ExcelWorksheet worksheet, int fromRow, int toColumn);
    }
}