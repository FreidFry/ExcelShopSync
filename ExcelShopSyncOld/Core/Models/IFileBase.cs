using OfficeOpenXml;

namespace ExcelShopSync.Core.Models
{
    public interface IFileBase
    {
        ExcelPackage ExcelPackage { get; set; }
        string FIleAndShopName { get; }
        string FileName { get; set; }
        List<PageBase> Pages { get; set; }
        string ShopName { get; set; }

    }
}