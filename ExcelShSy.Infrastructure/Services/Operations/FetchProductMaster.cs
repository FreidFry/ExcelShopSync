using ExcelShSy.Core.Abstracts;
using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Core.Interfaces.Excel;
using ExcelShSy.Core.Interfaces.Operations;
using ExcelShSy.Core.Interfaces.Storage;
using ExcelShSy.Infrastructure.Extensions;
using ExcelShSy.Infrastructure.Persistance.DefaultValues;
using OfficeOpenXml;

using ExcelShSy.Infrastructure.Persistance.ShopData.Mappings;
using ExcelShSy.Core.Interfaces.Shop;
using ExcelShSy.Core.Properties;

namespace ExcelShSy.Infrastructure.Services.Operations
{
    public class FetchProductMaster : BaseProductImporter, IFetchMasterProduct
    {
        public FetchProductMaster(IProductStorage dataProduct, IShopStorage shopStorage, ILogger logger) : base(dataProduct, shopStorage, logger)
        { }

        protected override void ProcessPage(IExcelSheet page)
        {
            var worksheet = page.Worksheet;

            var article = 0;
            var price = 0;
            var quantity = 0;
            var availability = 0;

            var articleComp = 0;
            var priceComp = 0;
            var quantityComp = 0;
            var availabilityComp = 0;


            foreach (var row in worksheet.GetFullRowRange())
            {
                var isHeader = false;

                var range = worksheet.GetRowValueColumnMap(row);

                article = article.GetColumnFromRange(range, ColumnConstants.Article, ref isHeader);
                price = price.GetColumnFromRange(range, ColumnConstants.Price, ref isHeader);
                quantity = quantity.GetColumnFromRange(range, ColumnConstants.Quantity, ref isHeader);
                availability = availability.GetColumnFromRange(range, ColumnConstants.Availability, ref isHeader);

                articleComp = articleComp.GetColumnFromRange(range, ColumnConstants.ComplectArticle, ref isHeader);
                priceComp = priceComp.GetColumnFromRange(range, ColumnConstants.ComplectPrice, ref isHeader);
                quantityComp = quantityComp.GetColumnFromRange(range, ColumnConstants.ComplectQuantity, ref isHeader);
                availabilityComp = availabilityComp.GetColumnFromRange(range, ColumnConstants.ComplectAvailability, ref isHeader);

                if (isHeader)
                {
                    Logger.LogInfo($"In {row} row headers");
                    continue;
                }
                if (article != 0)
                {
                    var current = worksheet.GetArticle(row, article);
                    if (current != null)
                        SetProductData(current, price, quantity, availability, row, worksheet);
                }

                if (articleComp != 0)
                {
                    var currentComp = worksheet.GetArticle(row, articleComp);
                    if (currentComp != null)
                        SetProductData(currentComp, priceComp, quantityComp, availabilityComp, row, worksheet);
                }
            }
        }

        private void SetProductData(string article, int priceCol, int qtyCol, int availCol, int row, ExcelWorksheet ws)
        {
            DataProduct.AddProductArticle(article);

            if (ProductProcessingOptions.ShouldSyncPrices && priceCol > 0)
            {
                var val = ws.GetDecimal(row, priceCol);
                if (val != null) DataProduct.AddProductPrice(article, (decimal)val);
            }

            if (ProductProcessingOptions.ShouldSyncQuantities && qtyCol > 0)
            {
                var val = ws.GetQuantity(row, qtyCol);
                if (val != null) DataProduct.AddProductQuantity(article, (decimal)val);
            }

            if (ProductProcessingOptions.ShouldSyncAvailability && availCol > 0)
            {
                var unidentified = ws.GetString(row, availCol);
                var val = IndetifyAvailability(unidentified);
                if (val != null) DataProduct.AddProductAvailability(article, val);
            }
        }

        static string? IndetifyAvailability(string? availability)
        {
            if (availability == null) return null;
            var result = AvailabilityMappingPriceList.Template
                .FirstOrDefault(x => x.Value
                                        .Contains(availability))
                                            .Key;
            if (result == null) return null;
            return result;
        }
    }
}
