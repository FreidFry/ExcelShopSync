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
    /// Synchronizes product quantities into target Excel files using previously fetched data.
    /// </summary>
    [Task(nameof(ProductProcessingOptions.ShouldSyncQuantities), 2)]
    public class SyncQuantity(IProductStorage dataProduct, IFileStorage fileStorage, IDatabaseSearcher databaseSearcher, ILocalizationService localizationService)
        : IExecuteOperation
    {
        /// <summary>
        /// Holds the current shop name being processed.
        /// </summary>
        private string _shopName = string.Empty;

        /// <summary>
        /// Gets the collection of error messages produced during execution.
        /// </summary>
        public List<string> Errors { get; } = [];

        /// <summary>
        /// Iterates through all target files and writes updated quantity values.
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
        /// Processes a single target file by updating quantity values across its sheets.
        /// </summary>
        /// <param name="file">The target file to process.</param>
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
        /// Updates quantity values on the provided worksheet using the stored product information.
        /// </summary>
        /// <param name="page">The worksheet abstraction to update.</param>
        private void ProcessPage(IExcelSheet page)
        {
            var worksheet = page.Worksheet;

            var headers = page.InitialHeadersTuple(ColumnConstants.Quantity);

            if (headers.AnyIsNullOrEmpty()) return;

            foreach (var row in worksheet.GetFullRowRangeWithoutFirstRow())
            {
                var localArticle = worksheet.GetArticle(row, headers.articleColumn);
                if (localArticle == null) continue;
                var article = databaseSearcher.SearchProduct(_shopName, localArticle);
                

                if (dataProduct.Quantity.TryGetValue(article, out var value))
                    worksheet.WriteCell(row, headers.neededColumn, dataProduct.Quantity[article]);
            }
        }
    }
}
