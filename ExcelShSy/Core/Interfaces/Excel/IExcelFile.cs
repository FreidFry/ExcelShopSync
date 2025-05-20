using OfficeOpenXml;

namespace ExcelShSy.Core.Interfaces.Excel
{
    public interface IExcelFile
    {
        string FilePath { get; set; }
        string FileName { get; set; }
        ExcelPackage ExcelPackage { get; set; }
        string ShopName { get; set; }

        List<IExcelPage> Pages { get; set; }

        void ShowInfo();
    }
}
