using ExcelShSy.Core.Attributes;
using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Core.Interfaces.DataBase;
using ExcelShSy.Core.Interfaces.Excel;
using ExcelShSy.Core.Interfaces.Operations;
using ExcelShSy.Core.Interfaces.Shop;
using ExcelShSy.Core.Interfaces.Storage;
using ExcelShSy.Core.Properties;
using ExcelShSy.Infrastructure.Extensions;
using ExcelShSy.Infrastructure.Persistence.DefaultValues;

namespace ExcelShSy.Infrastructure.Services
{
    /// <summary>
    /// Synchronizes product availability values into target Excel files and maps them using shop-specific rules.
    /// </summary>
    [Task(nameof(ProductProcessingOptions.ShouldSyncAvailability))]
    public class SyncAvailability(
        IProductStorage dataProduct,
        IFileStorage fileStorage,
        IShopStorage shopMappings,
        IDatabaseSearcher databaseSearcher,
        ILocalizationService localizationService)
        : IExecuteOperation
    {
        /// <summary>
        /// Tracks the name of the shop currently being processed.
        /// </summary>
        private string _shopName = string.Empty;

        /// <summary>
        /// Gets the collection of errors encountered during synchronization.
        /// </summary>
        public List<string> Errors { get; } = [];

        /// <summary>
        /// Iterates through all target files and applies availability updates.
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
        /// Updates availability values for the supplied file.
        /// </summary>
        /// <param name="file">The file to process.</param>
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
        /// Writes availability values for each row in the provided worksheet using the shop mapping.
        /// </summary>
        /// <param name="page">The worksheet abstraction to update.</param>
        private void ProcessPage(IExcelSheet page)
        {
            var shopTemplate = shopMappings.GetShopMapping(_shopName);
            var worksheet = page.Worksheet;

            var headers = page.InitialHeadersTuple(ColumnConstants.Availability);

            if (headers.AnyIsNullOrEmpty()) return;

            foreach (var row in worksheet.GetFullRowRangeWithoutFirstRow())
            {
                var localArticle = worksheet.GetArticle(row, headers.articleColumn);
                if (localArticle == null) continue;
                var article = databaseSearcher.SearchProduct(_shopName, localArticle);
                
                if (dataProduct.Availability.TryGetValue(article, out var value))
                    worksheet.WriteCell(row, headers.neededColumn, shopTemplate.AvailabilityMap[value]);
            }
        }
    }
}
