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
    /// Synchronizes discount start and end dates into target Excel files using stored product data.
    /// </summary>
    [Task(nameof(ProductProcessingOptions.ShouldSyncDiscountDate), 4)]
    public class SyncDiscountDate(
        IProductStorage dataProduct,
        IFileStorage fileStorage,
        ILogger logger,
        IShopStorage shopMapping,
        IDatabaseSearcher databaseSearcher,
        ILocalizationService localizationService)
        : IExecuteOperation
    {
        /// <summary>
        /// Defines the date format string required by the current shop.
        /// </summary>
        private string? _shopFormat;
        
        /// <summary>
        /// Stores the name of the shop currently being processed.
        /// </summary>
        private string _shopName = string.Empty;
        
        /// <summary>
        /// Gets the collection of errors recorded while synchronizing.
        /// </summary>
        public List<string> Errors { get; } = [];

        /// <summary>
        /// Iterates through all target files and synchronizes discount dates.
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
                _shopFormat = SetDataFormat(file.ShopName);
                _shopName = file.ShopName;
                ProcessFile(file);
            }
        }

        /// <summary>
        /// Fetches the date format configured for the specified shop.
        /// </summary>
        /// <param name="shopName">The shop whose format should be used.</param>
        /// <returns>The date format string.</returns>
        private string SetDataFormat(string shopName)
        {
            var shopTemplate = shopMapping.GetShopMapping(shopName);
            return shopTemplate!.DataFormat!;
        }

        /// <summary>
        /// Processes a single file by updating discount dates on each worksheet.
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
        /// Updates discount date columns on the provided worksheet.
        /// </summary>
        /// <param name="page">The worksheet abstraction to update.</param>
        private void ProcessPage(IExcelSheet page)
        {
            var worksheet = page.Worksheet;

            var articleCol = page.InitialHeadersTuple();
            var dataStart = 0;
            var dataEnd = 0;
            try
            {
                var startDate = page.InitialNeedColumn(ColumnConstants.DiscountFrom);
                dataStart = startDate;
            }
            catch
            {
                logger.Log($"No column {ColumnConstants.DiscountFrom} in {page.SheetName}");
            }
            try
            {
                var endDate = page.InitialNeedColumn(ColumnConstants.DiscountTo);
                dataEnd = endDate;
            }
            catch
            {
                logger.Log($"No column {ColumnConstants.DiscountTo} in {page.SheetName}");
            }

            if (dataStart == 0 && dataEnd == 0) return;

            var productTo = new List<string>();
            var errorDate = new List<string>();
            foreach (var row in worksheet.GetFullRowRangeWithoutFirstRow())
            {
                var localArticle = worksheet.GetArticle(row, articleCol);
                if (localArticle == null) continue;
                var article = databaseSearcher.SearchProduct(_shopName, localArticle);

                if (dataProduct.DiscountFrom.TryGetValue(article, out DateTime valueFrom) && dataStart != 0)
                    worksheet.WriteCell(row, dataStart, ConvertDate(valueFrom));
                if (dataProduct.DiscountTo.TryGetValue(article, out DateTime valueTo) && dataEnd != 0)
                    worksheet.WriteCell(row, dataEnd, ConvertDate(valueTo));
                else productTo.Add(article);

                if (valueFrom >= ProductProcessingOptions.MinDateActually && valueFrom < valueTo)
                    errorDate.Add(article);
            }

            var products = productTo
                .Union(errorDate)
                .Distinct()
                .ToList();

            logger.Log($"Products where errors {string.Join(",", products)}");
        }

        /// <summary>
        /// Converts the supplied date to the current shop's format.
        /// </summary>
        /// <param name="date">The date to convert.</param>
        /// <returns>The formatted date string.</returns>
        private string ConvertDate(DateTime date)
        {
            return date.ToString(_shopFormat);
        }
    }
}
