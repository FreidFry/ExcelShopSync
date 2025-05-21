using ExcelShSy.Core.Interfaces.Excel;
using ExcelShSy.Core.Services;
using ExcelShSy.Infrastracture.Persistance.Model;
using ExcelShSy.Infrastracture.Persistance.ShopData;
using OfficeOpenXml;

namespace ExcelShSy.Core.Extensions
{
    public static class ExcelPageExtensions
    {
        public static string GetShop(this IExcelPage? page)
        {
            var headerCount = page?.UndefinedHeaders?.Count;
            if (headerCount == null || headerCount == 0) return ShopNameConstant.Unknown;

            var ShopData = new ShopMappings().Shops;

            Dictionary<string, int> shopScore = [];
            var ShopKeys = ShopData.Keys;

            foreach (var shopKey in ShopKeys)
            {
                var shopColumn = ShopData[shopKey].columns;

                var score = page?.UndefinedHeaders?.Keys.Intersect(shopColumn).Count();
                shopScore.Add(shopKey, score ?? 0);
                if (score >= headerCount / 2 && headerCount > 10) return shopKey;
            }

            var shopName = shopScore.OrderByDescending(x => x.Value).First().Key;
            return shopScore[shopName] > 0 ? shopName : ShopNameConstant.Unknown;
        }
        public static string GetLanguague(this IExcelPage? page)
        {
            if (page?.UndefinedHeaders == null || page.UndefinedHeaders.Count == 0)
                return "unknown";

            var allText = string.Join(" ", page.UndefinedHeaders.Keys.Take(15).ToList());
            var detector = new LanguageDetector();
            return detector.Detect(allText);
        }
        public static List<IExcelPage> GetPages(ExcelPackage package)
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
