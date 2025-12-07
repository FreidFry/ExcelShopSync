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
    /// <summary>
    /// Synchronizes product discount values into target Excel files.
    /// </summary>
    [Task(nameof(ProductProcessingOptions.ShouldSyncDiscounts), 3)]
    public class SyncDiscount(IProductStorage dataProduct, IFileStorage fileStorage, IDatabaseSearcher databaseSearcher, ILocalizationService localizationService)
        : IExecuteOperation
    {
        /// <summary>
        /// Tracks the shop currently being processed.
        /// </summary>
        private string _shopName = string.Empty;

        /// <summary>
        /// Gets the list of errors produced during synchronization.
        /// </summary>
        public List<string> Errors { get; } = [];

        /// <summary>
        /// Applies discount updates across all target files.
        /// </summary>
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

        /// <summary>
        /// Updates discount values in the specified file.
        /// </summary>
        /// <param name="file">The file to modify.</param>
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

        /// <summary>
        /// Writes discount values for the given worksheet.
        /// </summary>
        /// <param name="page">The worksheet abstraction to update.</param>
        private void ProcessPage(IExcelSheet page)
        {

            var worksheet = page.Worksheet;

            var headers = page.InitialHeadersTuple(ColumnConstants.Discount);

            if (headers.AnyIsNullOrEmpty()) return;

            foreach (var row in worksheet.GetFullRowRangeWithoutFirstRow())
            {
                var localArticle = worksheet.GetArticle(row, headers.articleColumn);
                if (localArticle == null) continue;
                var article = databaseSearcher.SearchProduct(_shopName, localArticle);

                if (dataProduct.Discount.TryGetValue(article, out var value))
                    worksheet.WriteCell(row, headers.neededColumn, value);
            }
        }
    }
}
