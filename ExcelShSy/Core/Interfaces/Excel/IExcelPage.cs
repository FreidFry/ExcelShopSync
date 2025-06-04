using OfficeOpenXml;

namespace ExcelShSy.Core.Interfaces.Excel
{
    public interface IExcelPage
    {
        string PageName { get; set; }
        ExcelWorksheet ExcelWorksheet { get; set; }

        Dictionary<string, int>? UndefinedHeaders { get; set; }
        Dictionary<string, int>? Headers { get; set; }

        string ShowInfo();

    }
}
