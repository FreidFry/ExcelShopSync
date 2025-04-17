using ExcelShopSync.Core.Static;
using ExcelShopSync.Infrastructure.Persistence;
using ExcelShopSync.Services.Base;

namespace ExcelShopSync.Services.Price
{
    class PriceWithPriceList
    {
        readonly Dictionary<string, string> Prices = [];
        readonly Dictionary<string, string> Availability = [];
        private bool AvailabilityToo;

        public PriceWithPriceList(bool AvailabilityToo)
        {
            this.AvailabilityToo = AvailabilityToo;
            foreach (var source in FileManager.Source)
                foreach (var page in source.Pages)
                {
                    var worksheet = page.ExcelWorksheet;
                    if (worksheet?.Dimension == null)
                        continue;

                    int? articleColumn = null;
                    int? articleComplectColumn = null;
                    int? priceColumn = null;
                    int? priceComplectColumn = null;
                    int? AvailabilityColumn = null;
                    int? AvailabilityComplectColumn = null;

                    for (int row = worksheet.Dimension.Start.Row; row <= worksheet.Dimension.End.Row; row++)
                    {
                        bool headerFound = false;

                        for (int col = worksheet.Dimension.Start.Column; col <= worksheet.Dimension.End.Column; col++)
                        {
                            string? cellValue = worksheet.Cells[row, col].Value?.ToString()?.Trim();

                            if (string.IsNullOrWhiteSpace(cellValue))
                                continue;

                            //проверка на наличие заголовка в файле. свитч по ключам из PricelistTemplate
                            foreach (var kv in PricelistTemplate.pricelistTemplate)
                            {
                                if (kv.Value.Any(v => string.Equals(v, cellValue, StringComparison.OrdinalIgnoreCase)))
                                {
                                    switch (kv.Key)
                                    {
                                        case "Article":
                                            articleColumn = col;
                                            headerFound = true;
                                            break;
                                        case "ArticleComlect":
                                            articleComplectColumn = col;
                                            headerFound = true;
                                            break;
                                        case "Price":
                                            priceColumn = col;
                                            headerFound = true;
                                            break;
                                        case "PriceComlect":
                                            priceComplectColumn = col;
                                            headerFound = true;
                                            break;
                                        case "Availability":
                                            AvailabilityColumn = col;
                                            headerFound = true;
                                            break;
                                        case "AvailabilityComlect":
                                            AvailabilityComplectColumn = col;
                                            headerFound = true;
                                            break;

                                    }
                                    break;
                                }
                            }

                            if (headerFound) continue;

                            if (articleColumn != null && priceColumn != null)
                            {
                                string? article = worksheet.Cells[row, (int)articleColumn].Value?.ToString();
                                string? price = worksheet.Cells[row, (int)priceColumn].Value?.ToString();

                                if (!string.IsNullOrEmpty(article) && !Prices.ContainsKey(article) && !string.IsNullOrEmpty(price))
                                    Prices[article] = price;
                                if (AvailabilityToo)
                                    if (!string.IsNullOrEmpty(AvailabilityColumn.ToString()))
                                    {
                                        string? availability = worksheet.Cells[row, (int)AvailabilityColumn].Value?.ToString();
                                        if (!string.IsNullOrEmpty(availability))
                                            Availability[article] = availability;
                                    }
                            }

                            if (articleComplectColumn != null && priceComplectColumn != null)
                            {
                                string? articleComplect = worksheet.Cells[row, articleComplectColumn.Value].Value?.ToString();
                                string? priceComplect = worksheet.Cells[row, priceComplectColumn.Value].Value?.ToString();

                                if (!string.IsNullOrEmpty(articleComplect) && !Prices.ContainsKey(articleComplect) && !string.IsNullOrEmpty(priceComplect))
                                    Prices[articleComplect] = priceComplect;
                                if (AvailabilityToo)
                                    if (!string.IsNullOrEmpty(AvailabilityComplectColumn.ToString()))
                                    {
                                        string? availability = worksheet.Cells[row, (int)AvailabilityComplectColumn].Value?.ToString();
                                        if (!string.IsNullOrEmpty(availability))
                                            Availability[articleComplect] = availability;
                                    }
                            }


                        }
                    }
                }
        }

        public void Transfer()
        {
            foreach (var target in FileManager.Target)
                foreach (var page in target.Pages)
                {
                    if (page == null || page.Headers == null ||
                        !page.Headers.TryGetValue(ColumnKeys.Article, out int articleC) ||
                        !page.Headers.TryGetValue(ColumnKeys.Price, out int priceC))
                        continue;
                    var worksheet = page.ExcelWorksheet;
                    foreach (int row in Enumerable.Range(worksheet.Dimension.Start.Row + 1, worksheet.Dimension.End.Row - worksheet.Dimension.Start.Row))
                    {
                        string? article = worksheet.Cells[row, articleC].Value?.ToString();
                        if (article == null || !Prices.ContainsKey(article)) continue;
                        AssistanceMethodsExtend.FillCell(worksheet, row, priceC, Prices[article]);
                    }
                    if (AvailabilityToo)
                    {
                        if (page.Headers.TryGetValue(ColumnKeys.Availability, out int availabilityC))
                        {
                            foreach (int row in Enumerable.Range(worksheet.Dimension.Start.Row + 1, worksheet.Dimension.End.Row - worksheet.Dimension.Start.Row))
                            {
                                string? article = worksheet.Cells[row, articleC].Value?.ToString();
                                if (article == null || !Availability.ContainsKey(article)) continue;
                                AssistanceMethodsExtend.FillCell(worksheet, row, availabilityC, ShopTemplate.AvaibilityPref[target.ShopName][Availability[article]]);
                            }
                        }
                    }
                }
        }
    }
}
