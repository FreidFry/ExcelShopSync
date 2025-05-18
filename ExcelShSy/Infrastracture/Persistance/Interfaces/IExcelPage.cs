using OfficeOpenXml;

namespace ExcelShSy.Infrastracture.Persistance.Interfaces
{
    public interface IExcelPage
    {
        string PageName { get; set; }
        //ExcelWorksheet ExcelWorksheet { get; set; }

        Dictionary<string, int> Headers { get; set; }

        void ShowInfo();
    }
}
