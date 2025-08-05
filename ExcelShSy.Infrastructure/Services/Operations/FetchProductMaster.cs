using ExcelShSy.Core.Abstracts;
using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Core.Interfaces.Excel;
using ExcelShSy.Core.Interfaces.Operations;
using ExcelShSy.Core.Interfaces.Storage;
using ExcelShSy.Infrastructure.Extensions;
using ExcelShSy.Infrastructure.Persistance.DefaultValues;
using ExcelShSy.Properties;

using OfficeOpenXml;

using ExcelShSy.Infrastructure.Persistance.ShopData.Mappings;

namespace ExcelShSy.Infrastructure.Services.Operations
{
    public class FetchProductMaster : BaseProductImporter, IFetchMasterProduct
    {
        public FetchProductMaster(IProductStorage _dataProduct, IFileStorage _fileStorage, ILogger _logger) : base(_dataProduct, _fileStorage, _logger)
        { }

        protected override void ProcessPage(IExcelSheet page)
        {
            var worksheet = page.Worksheet;

            int article = 0;
            int price = 0;
            int quantity = 0;
            int availability = 0;

            int articleComp = 0;
            int priceComp = 0;
            int quantityComp = 0;
            int availabilityComp = 0;


            foreach (var row in worksheet.GetFullRowRange())
            {
                bool isHeader = false;

                var range = worksheet.GetRowValueColumnMap(row);

                article = article.GetColumnFromRange(range, ColumnConstants.Article);
                price = price.GetColumnFromRange(range, ColumnConstants.Price);
                quantity = quantity.GetColumnFromRange(range, ColumnConstants.Quantity);
                availability = availability.GetColumnFromRange(range, ColumnConstants.Availability);

                articleComp = articleComp.GetColumnFromRange(range, ColumnConstants.ComplectArticle);
                priceComp = priceComp.GetColumnFromRange(range, ColumnConstants.ComplectPrice);
                quantityComp = quantityComp.GetColumnFromRange(range, ColumnConstants.ComplectQuantity);
                availabilityComp = availabilityComp.GetColumnFromRange(range, ColumnConstants.ComplectAvailability);

                if (isHeader)
                {
                    _logger.LogInfo($"In {row} row headers");
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

        void SetProductData(string article, int priceCol, int qtyCol, int availCol, int row, ExcelWorksheet ws)
        {
            _dataProduct.AddProductArticle(article);

            if (ProductProcessingOptions.ShouldSyncPrices && priceCol > 0)
            {
                var val = ws.GetDecimal(row, priceCol);
                if (val != null) _dataProduct.AddProductPrice(article, (decimal)val);
            }

            if (ProductProcessingOptions.ShouldSyncQuantities && qtyCol > 0)
            {
                var val = ws.GetQuantity(row, qtyCol);
                if (val != null) _dataProduct.AddProductQuantity(article, (decimal)val);
            }

            if (ProductProcessingOptions.ShouldSyncAvailability && availCol > 0)
            {
                var undentif = ws.GetString(row, availCol);
                var val = IndentifyAvailability(undentif);
                if (val != null) _dataProduct.AddProductAvailability(article, val);
            }
        }

        static string? IndentifyAvailability(string? availability)
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
