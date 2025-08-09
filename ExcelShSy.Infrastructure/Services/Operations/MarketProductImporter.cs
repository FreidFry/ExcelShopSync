using ExcelShSy.Core.Abstracts;
using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Core.Interfaces.Excel;
using ExcelShSy.Core.Interfaces.Operations;
using ExcelShSy.Core.Interfaces.Shop;
using ExcelShSy.Core.Interfaces.Storage;
using ExcelShSy.Infrastructure.Extensions;
using ExcelShSy.Infrastructure.Persistance.DefaultValues;
using ExcelShSy.Properties;

using OfficeOpenXml;

namespace ExcelShSy.Infrastructure.Services.Operations
{
    public class MarketProductImporter : BaseProductImporter, IFetchMarketProduct
    {
        IShopTemplate _shopTemplate;
        public MarketProductImporter(IProductStorage _dataProduct, IFileStorage _fileStorage, ILogger _logger, IShopStorage _) : base(_dataProduct, _fileStorage, _, _logger)
        { 
         _shopTemplate = _shopStorage.GetShopMapping(shopName);
        }

        protected override void ProcessPage(IExcelSheet page)
        {
            var headers = page.MappedHeaders;
            if (headers.IsNullOrEmpty()) return;

            var articleCol = headers.TryGetValue(ColumnConstants.Article, out var a) ? a : 0;
            var priceCol = headers.TryGetValue(ColumnConstants.Price, out var p) ? p : 0;
            var quantityCol = headers.TryGetValue(ColumnConstants.Quantity, out var q) ? q : 0;
            var availabilityCol = headers.TryGetValue(ColumnConstants.Availability, out var av) ? av : 0;
            var discountCol = headers.TryGetValue(ColumnConstants.Discount, out var d) ? d : 0;
            var discountFromCol = headers.TryGetValue(ColumnConstants.DiscountFrom, out var df) ? df : 0;
            var discountToCol = headers.TryGetValue(ColumnConstants.DiscountTo, out var dt) ? dt : 0;
            var worksheet = page.Worksheet;

            if (articleCol == 0)
            {
                _logger.LogWarning($"{page.SheetName} not found article");
                return;
            }
            foreach (var row in worksheet.GetFullRowRangeWithoutFirstRow())
            {
                var article = worksheet.GetArticle(row, articleCol);
                if (string.IsNullOrEmpty(article))
                {
                    _logger.LogWarning($"In {row} row empty atricle");
                    continue;
                }

                SetProductData(article, priceCol, quantityCol, availabilityCol, discountCol, discountFromCol, discountToCol, row, worksheet);
            }
        }

        void SetProductData(string article, int priceCol, int qtyCol, int availCol, int Discount, int DiscountFrom, int DiscountTo, int row, ExcelWorksheet ws)
        {
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
                var val = ws.GetString(row, availCol);
                if (val != null) _dataProduct.AddProductAvailability(article,
                    _shopTemplate.AvailabilityMap
                    .FirstOrDefault(a => a.Value == val)
                    .Key ?? AvailabilityConstant.OnOrder);
            }

            if (ProductProcessingOptions.ShouldSyncDiscounts && Discount > 0)
            {
                var val = ws.GetDiscount(row, Discount);
                if (val != null) _dataProduct.AddProductDiscount(article, (decimal)val);
            }

            if (ProductProcessingOptions.ShouldSyncDiscountDate && DiscountFrom > 0)
            {
                var val = ws.GetDate(row, DiscountFrom);
                if (val != null) if (val > ProductProcessingOptions.MinDateActually) _dataProduct.AddProductDiscountFrom(article, (DateOnly)val);
            }

            if (ProductProcessingOptions.ShouldSyncDiscountDate && DiscountTo > 0)
            {
                var val = ws.GetDate(row, DiscountTo);
                if (val != null) if (val > ProductProcessingOptions.MinDateActually) _dataProduct.AddProductDiscountTo(article, (DateOnly)val);
            }
        }
    }
}
