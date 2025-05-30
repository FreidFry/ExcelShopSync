﻿using ExcelShopSync.Core.Static;
using ExcelShopSync.Infrastructure.Persistence;
using ExcelShopSync.Services.Base;
using static ExcelShopSync.Core.Static.ColumnKeys;

namespace ExcelShopSync.Services.Discount
{
    class FakeDiscount
    {
        public static void Transfer(double? priceIncreasePercentage)
        {
            if (priceIncreasePercentage < 100)
            {
                priceIncreasePercentage = (priceIncreasePercentage + 100) / 100;
            }
            else
            {
                AssistanceMethodsExtend.warning(priceIncreasePercentage >= 200, $"Are you sure you want a {priceIncreasePercentage}% increase?");
                priceIncreasePercentage /= 100;
            }

            foreach (var target in FileManager.Target)
            {
                foreach (var page in target.Pages)
                {
                    if (page == null || page.Headers == null ||
                        !page.Headers.TryGetValue(Article, out int articleC) ||
                        !page.Headers.TryGetValue(ColumnKeys.Price, out int priceC) ||
                        !page.Headers.TryGetValue(PriceOld, out int priceOldC))
                        continue;

                    var worksheet = page.ExcelWorksheet;
                    foreach (int row in Enumerable.Range(worksheet.Dimension.Start.Row + 1, worksheet.Dimension.End.Row - worksheet.Dimension.Start.Row))
                    {
                        string? article = worksheet.Cells[row, articleC].Value?.ToString();
                        string? price = worksheet.Cells[row, priceC].Value?.ToString();

                        if (article == null || price == null) continue;

                        double? priceValue = AssistanceMethodsExtend.PrepareExcelValue<double>(price) * priceIncreasePercentage;
                        if (priceValue.HasValue)
                        AssistanceMethodsExtend.FillCell(worksheet, row, priceOldC, priceValue);
                    }
                }
            }
        }
    }
}