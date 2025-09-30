using ExcelShSy.Core.Abstracts;
using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Core.Interfaces.Excel;
using ExcelShSy.Core.Interfaces.Operations;
using ExcelShSy.Core.Interfaces.Shop;
using ExcelShSy.Core.Interfaces.Storage;
using ExcelShSy.Core.Properties;
using ExcelShSy.Infrastructure.Extensions;
using ExcelShSy.Infrastructure.Persistence.DefaultValues;
using OfficeOpenXml;

namespace ExcelShSy.Infrastructure.Services.Operations
{
    public class MarketProductImporter : BaseProductImporter, IFetchMarketProduct
    {
        private readonly IShopTemplate? _shopTemplate;
        public MarketProductImporter(IProductStorage dataProduct, ILogger logger, IShopStorage _) : base(dataProduct, _, logger)
        {
            _shopTemplate = ShopStorage.GetShopMapping(ShopName);
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
                Logger.LogWarning($"{page.SheetName} not found article");
                return;
            }
            foreach (var row in worksheet.GetFullRowRangeWithoutFirstRow())
            {
                var article = worksheet.GetArticle(row, articleCol);
                if (string.IsNullOrEmpty(article))
                {
                    Logger.LogWarning($"In {row} row empty atricle");
                    continue;
                }

                SetProductData(article, priceCol, quantityCol, availabilityCol, discountCol, discountFromCol, discountToCol, row, worksheet);
            }
        }

        private void SetProductData(string article, int priceCol, int qtyCol, int availCol, int discount, int discountFrom, int discountTo, int row, ExcelWorksheet ws)
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
                var val = ws.GetString(row, availCol);
                if (val != null) DataProduct.AddProductAvailability(article,
                    _shopTemplate?.AvailabilityMap
                    .FirstOrDefault(a => a.Value == val)
                    .Key ?? AvailabilityConstant.OnOrder);
            }

            if (ProductProcessingOptions.ShouldSyncDiscounts && discount > 0)
            {
                var val = ws.GetDiscount(row, discount);
                if (val != null) DataProduct.AddProductDiscount(article, (decimal)val);
            }

            if (ProductProcessingOptions.ShouldSyncDiscountDate && discountFrom > 0)
            {
                var val = ws.GetDate(row, discountFrom);
                if (val != null) if (val > ProductProcessingOptions.MinDateActually) DataProduct.AddProductDiscountFrom(article, (DateOnly)val);
            }

            if (ProductProcessingOptions.ShouldSyncDiscountDate && discountTo > 0)
            {
                var val = ws.GetDate(row, discountTo);
                if (val != null) if (val > ProductProcessingOptions.MinDateActually) DataProduct.AddProductDiscountTo(article, (DateOnly)val);
            }
        }
    }
}
