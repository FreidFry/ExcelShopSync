using ExcelShSy.Core.Attributes;
using ExcelShSy.Core.Extensions;
using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Core.Interfaces.Excel;
using ExcelShSy.Core.Interfaces.Operations;
using ExcelShSy.Core.Interfaces.Storage;
using ExcelShSy.Infrastracture.Persistance.DefaultValues;

namespace ExcelShSy.Features.Services
{
    [Task("SyncPrice")]
    public class SyncPrice : IExecuteOperation
    {
        private readonly IDataProduct _dataProduct;
        private readonly IFileStorage _fileStorage;

        public SyncPrice(IDataProduct dataProduct, IFileStorage fileStorage)
        {
            _dataProduct = dataProduct;
            _fileStorage = fileStorage;
        }


        public void Execute()
        {
            foreach (var file in _fileStorage.Target) ProcessFile(file);
        }

        void ProcessFile(IExcelFile file)
        {
            foreach (var page in file.Pages) ProcessPage(page);
        }

        void ProcessPage(IExcelPage page)
        {
            var worksheet = page.ExcelWorksheet;

            var headers = page.InitialHeadersTuple(ColumnConstants.Price);

            if (headers.AnyIsNullOrEmpty()) return;

            var product = new List<string>();
            foreach (var row in worksheet.GetFullRowRangeWithoutFirstRow())
            {
                var article = worksheet.GetArticle(row, headers.articleColumn);

                if (article == null) continue;
                if (_dataProduct.Price.TryGetValue(article, out var value))
                worksheet.WriteCell(row, headers.neededColumn, value);
                else product.Add(article);
            }
        }
    }
}
