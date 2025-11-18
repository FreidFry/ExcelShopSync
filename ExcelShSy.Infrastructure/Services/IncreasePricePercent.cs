using ExcelShSy.Core.Attributes;
using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Core.Interfaces.Excel;
using ExcelShSy.Core.Interfaces.Operations;
using ExcelShSy.Core.Interfaces.Storage;
using ExcelShSy.Core.Properties;
using ExcelShSy.Infrastructure.Extensions;
using ExcelShSy.Infrastructure.Persistence.DefaultValues;
using static ExcelShSy.Infrastructure.Extensions.ExcelRangeExtensions;

namespace ExcelShSy.Infrastructure.Services
{
    /// <summary>
    /// Increases prices in target Excel files by the configured percentage.
    /// </summary>
    [Task(nameof(ProductProcessingOptions.ShouldIncreasePrices))]
    public class IncreasePricePercent(IFileStorage fileStorage, ILocalizationService localizationService)
        : IExecuteOperation
    {
        /// <summary>
        /// Gets the collection of errors generated while processing files.
        /// </summary>
        public List<string> Errors { get; } = [];

        /// <summary>
        /// Applies the price increase across all target files.
        /// </summary>
        public async Task Execute()
        {
            foreach (var file in fileStorage.Target) ProcessFile(file);
        }

        /// <summary>
        /// Handles price updates for a single file.
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
        /// Increases prices on the provided worksheet using the configured options.
        /// </summary>
        /// <param name="page">The worksheet abstraction to update.</param>
        private void ProcessPage(IExcelSheet page)
        {
            var worksheet = page.Worksheet;

            var headers = page.InitialHeadersTuple(ColumnConstants.Price);

            if (headers.AnyIsNullOrEmpty()) return;

            var oldPriceColumn = page.InitialNeedColumn(ColumnConstants.PriceOld);
            if (oldPriceColumn == 0) oldPriceColumn = page.InitialNeedColumn(ColumnConstants.Price);

            foreach (var row in worksheet.GetFullRowRangeWithoutFirstRow())
            {
                var price = worksheet.GetDecimal(row, headers.neededColumn);
                if (price == null) continue;
                var priceValue = (decimal)price * ProductProcessingOptions.priceIncreasePercentage;

                priceValue = RoundDecimal(priceValue, ProductProcessingOptions.ShouldRoundPrices ? 0 : 2);

                worksheet.WriteCell(row, oldPriceColumn, priceValue);
            }
        }
    }
}
