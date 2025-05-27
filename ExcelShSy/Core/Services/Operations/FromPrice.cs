using ExcelShSy.Core.Abstracts;
using ExcelShSy.Core.Extensions;
using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Core.Interfaces.Excel;
using ExcelShSy.Core.Interfaces.Operations;
using ExcelShSy.Core.Interfaces.Storage;
using ExcelShSy.Infrastracture.Persistance.DefaultValues;
using ExcelShSy.Properties;

using OfficeOpenXml;

namespace ExcelShSy.Core.Services.Operations
{
    public class FromPrice : GetProductFromBase, IFromPrice
    {
        public FromPrice(IDataProduct _dataProduct, IFileStorage _fileStorage) : base(_dataProduct, _fileStorage)
        { }
        protected override void ProcessPage(IExcelPage page)
        {
            var worksheet = page.ExcelWorksheet;

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

                articleComp = articleComp.GetColumnFromRange(range, ColumnConstants.CompectArticle);
                priceComp = priceComp.GetColumnFromRange(range, ColumnConstants.CompectPrice);
                quantityComp = quantityComp.GetColumnFromRange(range, ColumnConstants.CompectQuantity);
                availabilityComp = availabilityComp.GetColumnFromRange(range, ColumnConstants.CompectAvailability);

                if (isHeader) continue;
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
                if (val != null) _dataProduct.AddProductAvailability(article, val);
            }
        }
    }
}
