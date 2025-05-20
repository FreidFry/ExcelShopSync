using ExcelShSy.Core.Interfaces;
using OfficeOpenXml;
using System.IO;
using System.Windows;

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
            ExcelPackage = new ExcelPackage(path);
            Pages = GetPages(ExcelPackage);
            Language = LanguagueDetect();
            ShopName = IndetifyShop();
            ShowInfo();
        }

        public void ShowInfo()
        {
            foreach (var page in Pages)
                if (page.ShowInfo() != true) return; //if press ok => continue, else => stop
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

        string IndetifyShop()
        {
            List<string> shops = [];
            foreach(var page in Pages)
            {
                shops.Add(page.GetShop());
                if (shops.Count > 6) break;
            }

            var thisShop = shops.GroupBy(x => x)
                .OrderByDescending(g => g.Count())
                .First()
                .Key;

            return thisShop;
        }

        string LanguagueDetect()
        {
            List<string> languagues = [];
            foreach( var page in Pages)
            {
                languagues.Add(page.GetLanguague());
                if (languagues.Count > 15) break;
            }

            var thisLanguage = languagues.GroupBy(x => x)
                .OrderByDescending(g => g.Count())
                .First()
                .Key;

            return thisLanguage;
        }
    }
}
