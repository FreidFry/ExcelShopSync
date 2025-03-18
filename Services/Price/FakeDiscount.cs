using ExcelShopSync.Properties;
using ExcelShopSync.Services.Base;
using static ExcelShopSync.Properties.ShopBase.ColumnKeys;

namespace ExcelShopSync.Services.Price
{
    class FakeDiscount
    {
        //public FakeDiscount()
        //{
        //    foreach (var source in FileManager.Source)
        //    {
        //        foreach (var page in source.Pages)
        //        {
        //            if (page == null || page.Headers == null ||
        //                !page.Headers.TryGetValue(Article, out int articleC) ||
        //                !page.Headers.TryGetValue(ShopBase.ColumnKeys.Price, out int priceC))
        //                continue;
        //            var worksheet = page.ExcelWorksheet;
        //            foreach (int row in Enumerable.Range(worksheet.Dimension.Start.Row + 1, worksheet.Dimension.End.Row - worksheet.Dimension.Start.Row))
        //            {
        //                string? article = worksheet.Cells[row, articleC].Value?.ToString();
        //                string? price = worksheet.Cells[row, priceC].Value?.ToString();

        //                if (article == null || Prices.ContainsKey(article) || price == null)
        //                {
        //                    continue;
        //                }

        //                Prices[article] = price;
        //            }
        //        }
        //    }
        //}


        public void Transfer(double? priceIncreasePercentage)
        {
            if (priceIncreasePercentage < 100)
            {
                priceIncreasePercentage = (priceIncreasePercentage + 100) / 100;
            }
            else
            {
                AssistanceMethods.warning(priceIncreasePercentage >= 200, $"Are you sure you want a {priceIncreasePercentage}% increase?");
                priceIncreasePercentage /= 100;
            }

            foreach (var target in FileManager.Target)
            {
                foreach (var page in target.Pages)
                {
                    if (page == null || page.Headers == null ||
                        !page.Headers.TryGetValue(Article, out int articleC) ||
                        !page.Headers.TryGetValue(ShopBase.ColumnKeys.Price, out int priceC) ||
                        !page.Headers.TryGetValue(PriceOld, out int priceOldC))
                        continue;

                    var worksheet = page.ExcelWorksheet;
                    foreach (int row in Enumerable.Range(worksheet.Dimension.Start.Row + 1, worksheet.Dimension.End.Row - worksheet.Dimension.Start.Row))
                    {
                        string? article = worksheet.Cells[row, articleC].Value?.ToString();
                        string? price = worksheet.Cells[row, priceC].Value?.ToString();

                        if (article == null || price == null) continue;

                        double? priceValue = AssistanceMethods.PrepareExcelValue<double>(price) * priceIncreasePercentage;
                        AssistanceMethods.FillCell(worksheet, row, priceOldC, priceValue);
                    }
                }
            }
        }
    }
}