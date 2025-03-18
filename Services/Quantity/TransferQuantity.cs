using ExcelShopSync.Properties;
using ExcelShopSync.Services.Base;
using static ExcelShopSync.Properties.ShopBase.ColumnKeys;

namespace ExcelShopSync.Services.Quantity
{
    class TransferQuantity
    {
        readonly Dictionary<string, string> Quatnities = [];
        public TransferQuantity()
        {
            foreach (var source in FileManager.Source)
            {
                foreach (var page in source.Pages)
                {
                    if (page == null || page.Headers == null ||
                        !page.Headers.TryGetValue(Article, out int articleC) ||
                        !page.Headers.TryGetValue(ShopBase.ColumnKeys.Quantity, out int quantityC))
                        continue;
                    var worksheet = page.ExcelWorksheet;
                    foreach (int row in Enumerable.Range(worksheet.Dimension.Start.Row + 1, worksheet.Dimension.End.Row - worksheet.Dimension.Start.Row))
                    {
                        string? article = worksheet.Cells[row, articleC].Value?.ToString();
                        string? quantity = worksheet.Cells[row, quantityC].Value?.ToString();

                        if (article == null || Quatnities.ContainsKey(article) || quantity == null)
                        {
                            continue;
                        }
                        
                        Quatnities[article] = quantity;
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
                        !page.Headers.TryGetValue(ShopBase.ColumnKeys.Quantity, out int quantityC))
                        continue;

                    var worksheet = page.ExcelWorksheet;
                    foreach (int row in Enumerable.Range(worksheet.Dimension.Start.Row + 1, worksheet.Dimension.End.Row - worksheet.Dimension.Start.Row))
                    {
                        string? article = worksheet.Cells[row, articleC].Value?.ToString();
                        if (article == null || !Quatnities.ContainsKey(article)) continue;

                        AssistanceMethods.FillCell(worksheet, row, quantityC, Quatnities[article]);
                    }
                }
            }
        }
    }
}
