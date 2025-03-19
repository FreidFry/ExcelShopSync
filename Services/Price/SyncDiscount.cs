using ExcelShopSync.Services.Base;
using static ExcelShopSync.Properties.Settings;
using static ExcelShopSync.Modules.ColumnKeys;
using static ExcelShopSync.Properties.DataFormats;
using System.Windows;
using ExcelShopSync.Modules;

namespace ExcelShopSync.Services.Price
{
    class SyncDiscount
    {
        readonly Dictionary<string, Dictionary<string, string>> DiscountPrices = [];
        public SyncDiscount()
        {
            foreach (var source in FileManager.Source)
            {
                foreach (var page in source.Pages)
                {
                    if (page == null || page.Headers == null ||
                        !page.Headers.TryGetValue(Article, out int articleC) ||
                        !page.Headers.TryGetValue(Discount, out int discountC))
                        continue;
                    page.Headers.TryGetValue(DiscountFrom, out int discountFromC);
                    page.Headers.TryGetValue(DiscountTo, out int discountToC);

                    var worksheet = page.ExcelWorksheet;
                    foreach (int row in Enumerable.Range(worksheet.Dimension.Start.Row + 1, worksheet.Dimension.End.Row - worksheet.Dimension.Start.Row -1))
                    {
                        string? article = worksheet.Cells[row, articleC].Value?.ToString();
                        string? discount = worksheet.Cells[row, discountC].Value?.ToString();

                        if (article == null || DiscountPrices.ContainsKey(article) || discount == null)
                        {
                            continue;
                        }
                        DiscountPrices[article] = new() { { Discount, discount } };

                        if (discountFromC != 0)
                        {
#pragma warning disable CS8601 // Possible null reference assignment.
                            DiscountPrices[article][DiscountFrom] = worksheet.Cells[row, discountFromC].Value.ToString();
#pragma warning restore CS8601 // Possible null reference assignment.
                        }

                        if (discountToC != 0)
                        {
#pragma warning disable CS8601 // Possible null reference assignment.
                            DiscountPrices[article][DiscountTo] = worksheet.Cells[row, discountToC].Value.ToString();
#pragma warning restore CS8601 // Possible null reference assignment.
                        }
                    }
                }
            }
        }

        public void Transfer(DateTime? from, DateTime? to, string? time)
        {
            foreach (var target in FileManager.Target)
            {
                if (from == null || to == null) break;
                ParseDate(target.ShopName, (DateTime)from, (DateTime)to, time, out string? resultFrom, out string? resultTo);
                foreach (var page in target.Pages)
                {
                    if (page == null || page.Headers == null ||
                        !page.Headers.TryGetValue(Article, out int articleC) ||
                        !page.Headers.TryGetValue(Discount, out int discountC))
                        continue;
                    page.Headers.TryGetValue(DiscountFrom, out int discountFromC);
                    page.Headers.TryGetValue(DiscountTo, out int discountToC);

                    var worksheet = page.ExcelWorksheet;
                    foreach (int row in Enumerable.Range(worksheet.Dimension.Start.Row + 1, worksheet.Dimension.End.Row - worksheet.Dimension.Start.Row - 1))
                    {
                        string? article = worksheet.Cells[row, articleC].Value?.ToString();
                        if (article == null || !DiscountPrices.ContainsKey(article)) continue;

                        AssistanceMethods.FillCell(worksheet, row, discountC, DiscountPrices[article][Discount]);
                        if(discountFromC != 0)
                        {
                            AssistanceMethods.FillCell(worksheet, row, discountFromC, DiscountPrices[article][DiscountFrom]);
                        }
                        if (discountToC != 0)
                        {
                            AssistanceMethods.FillCell(worksheet, row, discountToC, DiscountPrices[article][DiscountTo]);
                        }
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
                        !page.Headers.TryGetValue(Discount, out int discountC))
                        continue;
                    page.Headers.TryGetValue(DiscountFrom, out int discountFromC);
                    page.Headers.TryGetValue(DiscountTo, out int discountToC);

                    var worksheet = page.ExcelWorksheet;
                    foreach (int row in Enumerable.Range(worksheet.Dimension.Start.Row + 1, worksheet.Dimension.End.Row - worksheet.Dimension.Start.Row - 1))
                    {
                        string? article = worksheet.Cells[row, articleC].Value?.ToString();
                        if (article == null || !DiscountPrices.ContainsKey(article)) continue;

                        AssistanceMethods.FillCell(worksheet, row, discountC, DiscountPrices[article][Discount]);
                        if (discountFromC != 0)
                        {
                            ParseDate(target.ShopName, DiscountPrices[article][DiscountFrom], out string? result);
                            AssistanceMethods.FillCell(worksheet, row, discountFromC, result ?? DateTime.Now.ToString(formats[target.ShopName]));
                        }
                        if (discountToC != 0)
                        {
                            ParseDate(target.ShopName, DiscountPrices[article][DiscountFrom], out string? result);
                            DateTime date = DateTime.Now;
                            date.AddDays(DefaultTimeOffset);
                            string ifEmpty = DateTime.Now.ToString(formats[target.ShopName]);
                            AssistanceMethods.FillCell(worksheet, row, discountToC, result ?? ifEmpty);
                        }
                    }
                }
            }
        }


        static void ParseDate(string shopname, DateTime from, DateTime to, string? time, out string? resultFrom, out string? resultTo)
        {
            if (to < from)
            {
                MessageBox.Show("Invalid date range");
                resultTo = from.ToString(formats[Shops.Unknown]);
            }

            if (TimeSpan.TryParse(time, out TimeSpan parsedTime))
            {
                from = from.Date.Add(parsedTime);
                to = to.Date.Add(parsedTime);
            }

            string format = formats[shopname];

            resultFrom = from.ToString(format);
            resultTo = to.ToString(format);
        }

        static void ParseDate(string shopname, DateTime date, string? time, out string? result)
        {
            if (TimeSpan.TryParse(time, out TimeSpan parsedTime))
            {
                date = date.Date.Add(parsedTime);
            }

            string format = formats[shopname];

            result = date.ToString(format);
        }

        static void ParseDate(string shopname, DateTime date, out string? result)
        {
            TimeSpan time = TimeSpan.Zero;
            date = date.Date.Add(time);

            string format = formats[shopname];

            result = date.ToString(format);
        }

        static void ParseDate(string shopname, string date, out string? result)
        {
            if (!DateTime.TryParse(date, out DateTime parsedDate))
            {
                result = null;
                return;
            }

            string format = formats[shopname];

            result = parsedDate.ToString(format);
        }

    }
}
