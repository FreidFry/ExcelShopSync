using ExcelShSy.Core.Interfaces.Excel;
using ExcelShSy.Core.Interfaces.Shop;

namespace ExcelShSy.Infrastructure.Extensions
{
    public static class ExcelFileExtensions
    {
        public static string IdentifyShop(this IList<IExcelSheet?> pages, IShopStorage shopStorage)
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

        public static string LanguageDetect(this IList<IExcelSheet?> pages)
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
