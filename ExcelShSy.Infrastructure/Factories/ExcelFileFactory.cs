using ExcelShSy.Core.Interfaces.Excel;
using ExcelShSy.Core.Interfaces.Shop;
using ExcelShSy.Infrastructure.Extensions;
using ExcelShSy.Infrastructure.Persistance.Model;

using OfficeOpenXml;

namespace ExcelShSy.Infrastructure.Factories
{
    public class ExcelFileFactory : IExcelFileFactory
    {
        private readonly IExcelPageFactory _excelPageFactory;
        private readonly IShopStorage _shopStorage;

        public ExcelFileFactory(IExcelPageFactory excelPageFactory, IShopStorage shopStorage)
        {
            _excelPageFactory = excelPageFactory;
            _shopStorage = shopStorage;
        }

        public IExcelFile Create(string path)
        {
            var file = new ExcelFile(path);

            file.SheetList = GetPages(file.ExcelPackage);

            file.ShopName = IndetifyShop(file.SheetList);
            file.Language = LanguageDetect(file.SheetList);

            return file;
        }

        List<IExcelSheet> GetPages(ExcelPackage package)
        {
            ExcelWorkbook workbook = package.Workbook;
            if (workbook == null)
                return [];
            List<IExcelSheet> pages = [];
            foreach (var page in workbook.Worksheets)
            {
                pages.Add(_excelPageFactory.Create(page));
            }
            return pages;
        }

        string IndetifyShop(List<IExcelSheet> pages)
        {
            List<string> shops = [];
            foreach (var page in pages)
            {
                shops.Add(page.GetShop(_shopStorage));
                if (shops.Count > 6) break;
            }

            var thisShop = shops.GroupBy(x => x)
                .OrderByDescending(g => g.Count())
                .First()
                .Key;

            return thisShop;
        }

        static string LanguageDetect(List<IExcelSheet> pages)
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
