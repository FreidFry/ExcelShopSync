using OfficeOpenXml;
using ExcelShopSync.Core.Models;
using ExcelShopSync.Core.Static;

namespace ExcelShopSync.Services.Base
{
    class IdentifyShop
    {
        public static string IdentifyShopByProbability(List<PageBase> Pages)
        {
            string? bestMatch = null;
            double highestMatchPercentage = 0;
            if(Pages.Count == 0)
            {
                return "Unknown";
            }
            foreach (var page in Pages)
            {
                ExcelWorksheet worksheet = page.ExcelWorksheet;
                if (worksheet == null || worksheet.Dimension == null)
                {
                    return "Unknown";
                }
                List<string> headers = [];
                for (int col = worksheet.Dimension.Start.Column; col <= worksheet.Dimension.End.Column; col++)
                {
                    string cell = worksheet.Cells[worksheet.Dimension.Start.Row, col].Text;
                    if (cell != null)
                        headers.Add(worksheet.Cells[worksheet.Dimension.Start.Row, col].Text);
                }

                foreach (var shop in ShopTemplate.Shops)
                {
                    var matchCount = headers.Intersect(shop.Value).Count();
                    double matchPercentage = (double)matchCount / shop.Value.Count * 100;

                    if (matchPercentage > highestMatchPercentage)
                    {
                        highestMatchPercentage = matchPercentage;
                        bestMatch = shop.Key;
                    }
                }
            }
            if (bestMatch == null)
            {
                return "Unknown";
            }
            return bestMatch;
        }
    }
}
