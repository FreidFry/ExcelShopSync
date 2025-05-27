using ExcelShSy.Core.Abstracts;
using ExcelShSy.Core.Extensions;
using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Core.Interfaces.Excel;
using ExcelShSy.Core.Interfaces.Operations;
using ExcelShSy.Core.Interfaces.Shop;
using ExcelShSy.Core.Interfaces.Storage;
using ExcelShSy.Infrastracture.Persistance.DefaultValues;
using ExcelShSy.Infrastracture.Persistance.ShopData;
using ExcelShSy.Properties;

using OfficeOpenXml;

namespace ExcelShSy.Core.Services.Operations
{
    public class FromSource : GetProductFromBase, IFromSource
    {
        IShopTemplate shopTemplate;
        public FromSource(IDataProduct _dataProduct, IFileStorage _fileStorage) : base(_dataProduct, _fileStorage)
        { }
        protected override void ProcessPage(IExcelPage page)
        {
            shopTemplate = new ShopMappings().GetShop(shopName);
            var headers = page.Headers;
            if (headers.IsNullOrEmpty()) return;

            var articleCol = headers.TryGetValue(ColumnConstants.Article, out var a) ? a : 0;
            var priceCol = headers.TryGetValue(ColumnConstants.Price, out var p) ? p : 0;
            var quantityCol = headers.TryGetValue(ColumnConstants.Quantity, out var q) ? q : 0;
            var availabilityCol = headers.TryGetValue(ColumnConstants.Availability, out var av) ? av : 0;
            var discountCol = headers.TryGetValue(ColumnConstants.Discount, out var d) ? d : 0;
            var discountFromCol = headers.TryGetValue(ColumnConstants.DiscountFrom, out var df) ? df : 0;
            var discountToCol = headers.TryGetValue(ColumnConstants.DiscountTo, out var dt) ? dt : 0;
            var worksheet = page.ExcelWorksheet;

            if (articleCol == 0) return;

            foreach (var row in worksheet.GetFullRowRangeWithoutFirstRow())
            {
                var article = worksheet.GetArticle(row, articleCol);
                if (string.IsNullOrEmpty(article)) continue;

                SetProductData(article, priceCol, quantityCol, availabilityCol, discountCol, discountFromCol, discountToCol, row, worksheet);
            }
        }

        void SetProductData(string article, int priceCol, int qtyCol, int availCol, int Discount, int DiscountFrom, int DiscountTo, int row, ExcelWorksheet ws)
        {
            if (GlobalSettings.SyncPrice && priceCol > 0)
            {
                var val = ws.GetDecimal(row, priceCol);
                if (val != null) _dataProduct.AddProductPrice(article, (decimal)val);
            }

            if (GlobalSettings.SyncQuantity && qtyCol > 0)
            {
                var val = ws.GetQuantity(row, qtyCol);
                if (val != null) _dataProduct.AddProductQuantity(article, (decimal)val);
            }

            if (GlobalSettings.SyncAvailability && availCol > 0)
            {
                var val = ws.GetString(row, availCol);
                if (val != null) _dataProduct.AddProductAvailability(article,
                    shopTemplate.Availability
                    .FirstOrDefault(a => a.Value == val)
                    .Key ?? AvailabilityConstant.OnOrder);
            }

            if (GlobalSettings.SyncDiscount && Discount > 0)
            {
                var val = ws.GetDiscount(row, Discount);
                if (val != null) _dataProduct.AddProductDiscount(article, (decimal)val);
            }
            if (GlobalSettings.DiscountDate && DiscountFrom > 0)
            {
                var val = ws.GetDate(row, DiscountFrom);
                if (val != null) if (val > GlobalSettings.MinDateActually ) _dataProduct.AddProductDiscountFrom(article, (DateOnly)val);
            }
            if (GlobalSettings.DiscountDate && DiscountTo > 0)
            {
                var val = ws.GetDate(row, DiscountTo);
                if (val != null) if (val > GlobalSettings.DiscountStartDate) _dataProduct.AddProductDiscountTo(article, (DateOnly)val);
            }
        }
    }
}
