using ExcelShSy.Core.Interfaces.Excel;
using ExcelShSy.Core.Interfaces.Shop;

namespace ExcelShSy.Infrastructure.Extensions
{
    /// <summary>
    /// Provides helper methods for analyzing collections of Excel sheets.
    /// </summary>
    public static class ExcelFileExtensions
    {
        /// <summary>
        /// Identifies the shop associated with the provided set of sheets.
        /// </summary>
        /// <param name="pages">The sheets to analyze.</param>
        /// <param name="shopStorage">The shop storage used to match headers.</param>
        /// <returns>The inferred shop name.</returns>
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

        /// <summary>
        /// Determines the predominant language across the provided sheets.
        /// </summary>
        /// <param name="pages">The sheets to analyze.</param>
        /// <returns>The detected language identifier.</returns>
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
