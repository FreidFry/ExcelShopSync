using ExcelShSy.Core.Extensions;
using ExcelShSy.Core.Interfaces.Excel;
using ExcelShSy.Infrastracture.Persistance.Model;

using OfficeOpenXml;

namespace ExcelShSy.Core.Factorys
{
    public class ExcelFileFactory : IExcelFileFactory
    {
        private readonly IExcelPageFactory _excelPageFactory;

        public ExcelFileFactory(IExcelPageFactory excelPageFactory)
        {
            _excelPageFactory = excelPageFactory;
        }

        public ExcelFile Create(string path)
        {
            var file = new ExcelFile(path);

            file.Pages = GetPages(file.ExcelPackage);

            file.ShopName = IndetifyShop(file.Pages);
            file.Language = LanguagueDetect(file.Pages);

            return file;
        }

        List<IExcelPage> GetPages(ExcelPackage package)
        {
            ExcelWorkbook workbook = package.Workbook;
            if (workbook == null)
                return [];
            List<IExcelPage> pages = [];
            foreach (var page in workbook.Worksheets)
            {
                pages.Add(_excelPageFactory.Create(page));
            }
            return pages;
        }

        static string IndetifyShop(List<IExcelPage> pages)
        {
            List<string> shops = [];
            foreach (var page in pages)
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

        static string LanguagueDetect(List<IExcelPage> pages)
        {
            List<string> languagues = [];
            foreach (var page in pages)
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
