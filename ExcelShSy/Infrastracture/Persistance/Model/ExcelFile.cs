using ExcelShSy.Core.Interfaces;
using OfficeOpenXml;
using System.IO;

namespace ExcelShSy.Infrastracture.Persistance.Model
{
    public class ExcelFile : IExcelFile
    {
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string ShopName { get; set; }
        public string Language { get; set; }
        public ExcelPackage ExcelPackage { get; set; }
        public List<IExcelPage> Pages { get; set; }

        public ExcelFile(string path)
        {
            FilePath = path;
            FileName = Path.GetFileName(path);
            ShopName = "Shop";
            Language = "ua";
            ExcelPackage = new ExcelPackage(path);
            Pages = GetPages(ExcelPackage);
        }

        public void ShowInfo()
        {
            foreach (var page in Pages)
                page.ShowInfo();
        }

        List<IExcelPage> GetPages(ExcelPackage package)
        {
            ExcelWorkbook workbook = package.Workbook;
            if (workbook == null)
                return [];
            List<IExcelPage> pages = [];
            foreach (var page in workbook.Worksheets)
            {
                pages.Add(new ExcelPage(page));
            }
            return pages;
        }

        
    }
}
