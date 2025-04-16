using ExcelShopSync.Core.Static;
using ExcelShopSync.Infrastructure.Persistence;
using ExcelShopSync.Services.Base;
using static ExcelShopSync.Core.Static.ColumnKeys;

namespace ExcelShopSync.Services.Price
{
    class TransferPrice
    {
        readonly Dictionary<string, string> Prices = [];
        public TransferPrice()
        {
            foreach (var source in FileManager.Source)
            {
                foreach (var page in source.Pages)
                {
                    if (page == null || page.Headers == null ||
                        !page.Headers.TryGetValue(Article, out int articleC) ||
                        !page.Headers.TryGetValue(ColumnKeys.Price, out int priceC))
                        continue;
                    var worksheet = page.ExcelWorksheet;
                    foreach (int row in Enumerable.Range(worksheet.Dimension.Start.Row + 1, worksheet.Dimension.End.Row - worksheet.Dimension.Start.Row))
                    {
                        string? article = worksheet.Cells[row, articleC].Value?.ToString();
                        string? price = worksheet.Cells[row, priceC].Value?.ToString();

                        if (article == null || Prices.ContainsKey(article) || price == null)
                        {
                            continue;
                        }

                        Prices[article] = price;
                    }
                }
            }
        }


        public void Transfer()
        {
            foreach (var target in FileManager.Target)
            {
                foreach (var page in target.Pages)
                {
                    if (page == null || page.Headers == null ||
                        !page.Headers.TryGetValue(Article, out int articleC) ||
                        !page.Headers.TryGetValue(ColumnKeys.Price, out int priceC))
                        continue;

                    var worksheet = page.ExcelWorksheet;
                    foreach (int row in Enumerable.Range(worksheet.Dimension.Start.Row + 1, worksheet.Dimension.End.Row - worksheet.Dimension.Start.Row))
                    {
                        string? article = worksheet.Cells[row, articleC].Value?.ToString();
                        if (article == null || !Prices.ContainsKey(article)) continue;
                        
                        AssistanceMethodsExtend.FillCell(worksheet, row, priceC, Prices[article]);
                    }
                }
            }
        }
    }
}
