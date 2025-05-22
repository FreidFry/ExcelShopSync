using ExcelShSy.Core.Interfaces.Excel;

namespace ExcelShSy.Core.Extensions
{
    public static class ExcelFileExtensions
    {
        public static string IndetifyShop(this IList<IExcelPage?> pages)
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

        public static string LanguagueDetect(this IList<IExcelPage?> pages)
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
