using ExcelShSy.Core.Extensions;
using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Core.Interfaces.Excel;
using ExcelShSy.Core.Interfaces.Operations;
using ExcelShSy.Core.Interfaces.Storage;
using ExcelShSy.Infrastracture.Persistance.DefaultValues;

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
            foreach (var file in _fileStorage.Source) ProcessFile(file);
        }

        void ProcessFile(IExcelFile file)
        {
            foreach (var page in file.Pages) ProcessPage(page);
        }



        void ProcessPage(IExcelPage page)
        {
            var headers = page.Headers;
            string[] _ = [ColumnConstants.Article, ColumnConstants.Price];
            if (headers == null
                || headers.IsNullOrEmpty()
                || !headers.HasRequiredKeys(_)) return;

            var articleCol = headers[ColumnConstants.Article];
            var priceCol = headers[ColumnConstants.Price];
            var worksheet = page.ExcelWorksheet;

            foreach (var row in worksheet.GetFullRowRangeWithoutFirstRow())
            {
                var article = worksheet.GetArticle(row, articleCol);
                var price = worksheet.GetDecimal(row, priceCol);
                if (string.IsNullOrEmpty(article) || price == null) continue;
                _dataProduct.AddProductPrice(article, (decimal)price);
            }

        }
    }
}
