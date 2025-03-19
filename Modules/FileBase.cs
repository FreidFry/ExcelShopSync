using OfficeOpenXml;
using System.IO;

using static ExcelShopSync.Services.Base.IdentifyShop;
namespace ExcelShopSync.Modules
{
    class FileBase
    {
        public string FileName { get; set; }
        public string ShopName { get; set; }
        public ExcelPackage ExcelPackage { get; set; }
        public List<PageBase> Pages { get; set; } = [];

        public FileBase(string path)
        {
            FileName = Path.GetFileName(path);
            ExcelPackage = new ExcelPackage(path);
            foreach(var page in ExcelPackage.Workbook.Worksheets)
            {
                Pages.Add(new PageBase(page));
            }
            ShopName = IdentifyShopByProbability(Pages);
        }
    }
}
