using ExcelShopSync.Properties;
using ExcelShopSync.Services.Base;
using static ExcelShopSync.Properties.Settings;
using static ExcelShopSync.Properties.ShopBase.ColumnKeys;

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
                            DiscountPrices[article][DiscountFrom] = worksheet.Cells[row, discountFromC].Value.ToString();
                        }

                        if (discountToC != 0)
                        {
                            DiscountPrices[article][DiscountTo] = worksheet.Cells[row, discountToC].Value.ToString();
                        }
                    }
                }
            }
        }

        public void Transfer(DateTime? from, DateTime? to, string? time)
        {
            foreach (var target in FileManager.Target)
            {
                if (from == null || to == null) throw new ArgumentException("Invalid date range");
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
                            AssistanceMethods.FillCell(worksheet, row, discountFromC, result ?? DateTime.Now.ToString(ShopBase.formats[target.ShopName]));
                        }
                        if (discountToC != 0)
                        {
                            ParseDate(target.ShopName, DiscountPrices[article][DiscountFrom], out string? result);
                            DateTime date = DateTime.Now;
                            date.AddDays(DefaultTimeOffset);
                            string ifempty = DateTime.Now.ToString(ShopBase.formats[target.ShopName]);
                            AssistanceMethods.FillCell(worksheet, row, discountToC, result ?? date.ToString(ShopBase.formats[target.ShopName]));
                        }
                    }
                }
            }
        }


        void ParseDate(string shopname, DateTime from, DateTime to, string? time, out string? resultFrom, out string? resultTo)
        {
            if (to < from) throw new ArgumentException("Invalid date range");

            if (TimeSpan.TryParse(time, out TimeSpan parsedTime))
            {
                from = from.Date.Add(parsedTime);
                to = to.Date.Add(parsedTime);
            }
            else
            {
                throw new ArgumentException("Invalid time format");
            }

            string format = ShopBase.formats[shopname];

            resultFrom = from.ToString(format);
            resultTo = to.ToString(format);
        }

        void ParseDate(string shopname, DateTime date, string? time, out string? result)
        {
            if (TimeSpan.TryParse(time, out TimeSpan parsedTime))
            {
                date = date.Date.Add(parsedTime);
            }
            else
            {
                throw new ArgumentException("Invalid time format");
            }

            string format = ShopBase.formats[shopname];

            result = date.ToString(format);
        }

        void ParseDate(string shopname, DateTime date, out string? result)
        {
            TimeSpan time = TimeSpan.Zero;
            date = date.Date.Add(time);

            string format = ShopBase.formats[shopname];

            result = date.ToString(format);
        }

        void ParseDate(string shopname, string date, out string? result)
        {
            if (!DateTime.TryParse(date, out DateTime parsedDate))
            {
                result = null;
                return;
            }

            string format = ShopBase.formats[shopname];

            result = parsedDate.ToString(format);
        }

    }
}
