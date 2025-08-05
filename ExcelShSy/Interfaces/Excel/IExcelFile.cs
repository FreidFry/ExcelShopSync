using OfficeOpenXml;

namespace ExcelShSy.Core.Interfaces.Excel
{
    public interface IExcelFile
    {
        string FileLocation { get; set; }
        string FileName { get; set; }
        ExcelPackage ExcelPackage { get; set; }
        string ShopName { get; set; }

        List<IExcelSheet> SheetList { get; set; }

        void ShowFileDetails();
    }
}
