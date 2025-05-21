using ExcelShSy.Core.Extensions;
using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Core.Interfaces.Operations;
using ExcelShSy.Core.Interfaces.Storage;
using ExcelShSy.Infrastracture.Persistance.DefaultValues;

using System.Windows;

namespace ExcelShSy.Core.Services.Operations
{
    public class GetPricesFromSource : IGetPricesFromSource
    {
        readonly IDataProduct _dataProduct;
        readonly IFileStorage _fileStorage;
        public GetPricesFromSource(IDataProduct dataProduct, IFileStorage fileStorage)
        {
            _dataProduct = dataProduct;
            _fileStorage = fileStorage;
        }

        public void GetAllPrice()
        {
            foreach (var file in _fileStorage.Source)
                foreach (var page in file.Pages)
                {
                    var headers = page.Headers;
                    string[] _ = [ColumnConstants.Article, ColumnConstants.Price];
                    if (headers == null
                        || headers.IsNullOrEmpty()
                        || !headers.HasRequiredKeys(_)) continue;

                    var articleCol = headers[ColumnConstants.Article];
                    var priceCol = headers[ColumnConstants.Price];
                    var worksheet = page.ExcelWorksheet;

                    foreach (var row in page.GetRowRange())
                    {
                        var article = worksheet.GetArticle(row, articleCol);
                        var price = worksheet.GetDecimal(row, priceCol);
                        if (string.IsNullOrEmpty(article) || price == null) continue;
                        _dataProduct.AddProductPrice(article, (decimal)price);
                    }
                }
            MessageBox.Show(string.Join('\n', _dataProduct.Price));
        }
    }
}
