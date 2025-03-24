using OfficeOpenXml;
using System.IO;
using System.Windows;
using static ExcelShopSync.Services.Base.IdentifyShop;

namespace ExcelShopSync.Modules
{
    public class FileBase
    {
        public string FileName { get; set; }
        public string ShopName { get; set; }
        public ExcelPackage ExcelPackage { get; set; }
        public List<PageBase> Pages { get; set; } = [];
        public string FIleAndShopName => $"{FileName}\n({ShopName})";

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

        public void ShowInfo()
        {
            foreach(var page in Pages)
            {
                if (page.Headers != null)
                {
                    MessageBox.Show($"{page.PageName}\n\n" +
                        $"{string.Join("\n", page.Headers.Select(kv => $"{kv.Key}: {kv.Value}"))}");
                }
                else
                {
                    MessageBox.Show($"{page.PageName}\n\nHeaders is null.");
                }
            }
        }
    }
}
