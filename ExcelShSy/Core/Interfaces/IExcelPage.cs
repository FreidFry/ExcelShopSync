using OfficeOpenXml;

namespace ExcelShSy.Core.Interfaces
{
    public interface IExcelPage
    {
        string PageName { get; set; }
        ExcelWorksheet ExcelWorksheet { get; set; }

        List<string> UndefinedHeaders { get; set; }
        Dictionary<string, int> Headers { get; set; }

        void ShowInfo();

        string GetShop();

    }
}
