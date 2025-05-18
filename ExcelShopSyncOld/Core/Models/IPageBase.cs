using OfficeOpenXml;

namespace ExcelShopSync.Core.Models
{
    public interface IPageBase
    {
        ExcelWorksheet ExcelWorksheet { get; }
        Dictionary<string, int>? Headers { get; set; }
        string PageName { get; }
    }
}