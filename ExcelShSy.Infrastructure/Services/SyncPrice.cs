using ExcelShSy.Core.Attributes;
using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Core.Interfaces.DataBase;
using ExcelShSy.Core.Interfaces.Excel;
using ExcelShSy.Core.Interfaces.Operations;
using ExcelShSy.Core.Interfaces.Storage;
using ExcelShSy.Core.Properties;
using ExcelShSy.Infrastructure.Extensions;
using ExcelShSy.Infrastructure.Persistence.DefaultValues;

namespace ExcelShSy.Infrastructure.Services
{
    [Task(nameof(ProductProcessingOptions.ShouldSyncPrices))]
    public class SyncPrice(IProductStorage dataProduct, IFileStorage fileStorage, IDatabaseSearcher databaseSearcher, ILocalizationService localizationService)
        : IExecuteOperation
    {
        private string _shopName = string.Empty;

        public List<string> Errors { get; } = [];

        public async Task Execute()
        {
            foreach (var file in fileStorage.Target)
            {
                if (file.ShopName == null)
                {
                    var msg = localizationService.GetErrorString("ErrorNoShopName");
                    var formatted = string.Format(msg, file.FileName);
                    Errors.Add(formatted);
                    continue;
                }
                _shopName = file.ShopName;
                ProcessFile(file);
            }
        }

        private void ProcessFile(IExcelFile file)
        {
            if (file.SheetList == null)
            {
                var msg = localizationService.GetErrorString("ErrorNoSheets");
                var formatted = string.Format(msg, file.FileName);
                Errors.Add(formatted);
                return;
            }
            foreach (var page in file.SheetList) OperationWrapper.Try(() => ProcessPage(page), Errors, file.FileName);
        }

        private void ProcessPage(IExcelSheet page)
        {
            var worksheet = page.Worksheet;

            var headers = page.InitialHeadersTuple(ColumnConstants.Price);

            if (headers.AnyIsNullOrEmpty()) return;

            foreach (var row in worksheet.GetFullRowRangeWithoutFirstRow())
            {
                var localArticle = worksheet.GetArticle(row, headers.articleColumn);
                if (localArticle == null) continue;
                
                var article = databaseSearcher.SearchProduct(_shopName, localArticle);
                
                if (dataProduct.Price.TryGetValue(article, out var value))
                    worksheet.WriteCell(row, headers.neededColumn, value);
            }
        }
    }
}
