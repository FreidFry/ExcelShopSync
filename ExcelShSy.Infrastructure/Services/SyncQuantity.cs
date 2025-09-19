using ExcelShSy.Core.Attributes;
using ExcelShSy.Core.Interfaces.Excel;
using ExcelShSy.Core.Interfaces.Operations;
using ExcelShSy.Core.Interfaces.Storage;
using ExcelShSy.Core.Properties;
using ExcelShSy.Infrastructure.Extensions;
using ExcelShSy.Infrastructure.Persistance.DefaultValues;

namespace ExcelShSy.Infrastructure.Services
{
    [Task(nameof(ProductProcessingOptions.ShouldSyncQuantities))]
    public class SyncQuantity : IExecuteOperation
    {
        private readonly IProductStorage _dataProduct;
        private readonly IFileStorage _fileStorage;

        public SyncQuantity(IProductStorage dataProduct, IFileStorage fileStorage)
        {
            _dataProduct = dataProduct;
            _fileStorage = fileStorage;
        }

        public List<string> Errors { get; } = [];

        public void Execute()
        {
            foreach (var file in _fileStorage.Target) ProcessFile(file);
        }

        void ProcessFile(IExcelFile file)
        {
            foreach (var page in file.SheetList) OperationWraper.Try(() => ProcessPage(page), Errors, file.FileName);
        }

        void ProcessPage(IExcelSheet page)
        {
            var worksheet = page.Worksheet;

            var headers = page.InitialHeadersTuple(ColumnConstants.Quantity);

            if (headers.AnyIsNullOrEmpty()) return;

            var product = new List<string>();
            foreach (var row in worksheet.GetFullRowRangeWithoutFirstRow())
            {
                var article = worksheet.GetArticle(row, headers.articleColumn);

                if (article == null) continue;
                if (_dataProduct.Quantity.TryGetValue(article, out var value))
                worksheet.WriteCell(row, headers.neededColumn, _dataProduct.Quantity[article]);
                else product.Add(article);

            }
        }
    }
}
