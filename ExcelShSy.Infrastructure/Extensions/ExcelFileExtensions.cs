using ExcelShSy.Core.Interfaces.Excel;
using ExcelShSy.Core.Interfaces.Shop;

namespace ExcelShSy.Infrastructure.Extensions
{
    public static class ExcelFileExtensions
    {
        public static string IndetifyShop(this IList<IExcelSheet?> pages, IShopStorage shopStorage)
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

        public static string LanguagueDetect(this IList<IExcelSheet?> pages)
        {
            List<string> languagues = [];
            foreach (var page in pages)
            {
                languagues.Add(page.GetLanguage());
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
