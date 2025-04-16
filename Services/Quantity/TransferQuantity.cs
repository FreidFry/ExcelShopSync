using ExcelShopSync.Services.Base;
using static ExcelShopSync.Core.Static.ColumnKeys;
using static ExcelShopSync.Core.Static.AvailabilityKeys;
using ExcelShopSync.Core.Static;
using ExcelShopSync.Infrastructure.Persistence;

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
                        !page.Headers.TryGetValue(ColumnKeys.Quantity, out int quantityC))
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

        public void Transfer(double readyToGo = double.MaxValue)
        {
            foreach (var target in FileManager.Target)
            {
                foreach (var page in target.Pages)
                {
                    if (page == null || page.Headers == null ||
                        !page.Headers.TryGetValue(Article, out int articleC) ||
                        !page.Headers.TryGetValue(ColumnKeys.Quantity, out int quantityC))
                        continue;
                    page.Headers.TryGetValue(Availability, out int availabilityC);

                    var worksheet = page.ExcelWorksheet;
                    foreach (int row in Enumerable.Range(worksheet.Dimension.Start.Row + 1, worksheet.Dimension.End.Row - worksheet.Dimension.Start.Row))
                    {
                        string? article = worksheet.Cells[row, articleC].Value?.ToString();
                        if (article == null || !Quatnities.ContainsKey(article)) continue;

                        AssistanceMethodsExtend.FillCell(worksheet, row, quantityC, Quatnities[article]);
                        if (availabilityC != 0)
                        {
                            AssistanceMethodsExtend.FillCell(worksheet, row, availabilityC, double.Parse(Quatnities[article]) > readyToGo
                                ? ShopTemplate.AvaibilityPref[target.ShopName][ReadyToGo]
                                : ShopTemplate.AvaibilityPref[target.ShopName][InStock]);
                        }
                    }
                }
            }
        }
    }
}
