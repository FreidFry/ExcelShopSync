using ExcelShSy.Core.Interfaces.Excel;
using ExcelShSy.Core.Interfaces.Shop;
using ExcelShSy.Infrastructure.Extensions;
using ExcelShSy.Infrastructure.Persistence.Model;
using OfficeOpenXml;

namespace ExcelShSy.Infrastructure.Factories
{
    public class ExcelFileFactory(IExcelPageFactory excelPageFactory, IShopStorage shopStorage)
        : IExcelFileFactory
    {
        public IExcelFile Create(string path)
        {
            var file = new ExcelFile(path);

            file.SheetList = GetPages(file.ExcelPackage);

            file.ShopName = IndetifyShop(file.SheetList);
            file.Language = LanguageDetect(file.SheetList);

            return file;
        }

        private List<IExcelSheet> GetPages(ExcelPackage package)
        {
            ExcelWorkbook workbook = package.Workbook;
            if (workbook == null)
                return [];
            List<IExcelSheet> pages = [];
            pages.AddRange(workbook.Worksheets.Select(excelPageFactory.Create));
            return pages;
        }

        private string IndetifyShop(List<IExcelSheet> pages)
        {
            List<string> shops = [];
            foreach (var page in pages)
            {
                shops.Add(page.GetShop(shopStorage));
                if (shops.Count > 6) break;
            }

            var thisShop = shops.GroupBy(x => x)
                .OrderByDescending(g => g.Count())
                .First()
                .Key;

            return thisShop;
        }

        private static string LanguageDetect(List<IExcelSheet> pages)
        {
            List<string> languages = [];
            foreach (var page in pages)
            {
                languages.Add(page.GetLanguage());
                if (languages.Count > 15) break;
            }

            var thisLanguage = languages.GroupBy(x => x)
                .OrderByDescending(g => g.Count())
                .First()
                .Key;

            return thisLanguage;
        }

    }
}
