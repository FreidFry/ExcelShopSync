using ExcelShSy.Core.Attributes;
using ExcelShSy.Core.Interfaces.Excel;
using ExcelShSy.Core.Interfaces.Operations;
using ExcelShSy.Core.Interfaces.Storage;
using ExcelShSy.Infrastructure.Extensions;
using ExcelShSy.Infrastructure.Persistance.DefaultValues;
using ExcelShSy.Properties;
using static ExcelShSy.Infrastructure.Extensions.ExcelRangeExtensions;

namespace ExcelShSy.Infrastructure.Services
{
    [Task(nameof(ProductProcessingOptions.ShouldIncreasePrices))]
    public class IncreasePricePercent : IExecuteOperation
    {
        private readonly IFileStorage _fileStorage;

        public IncreasePricePercent(IFileStorage fileStorage)
        {
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

            var headers = page.InitialHeadersTuple(ColumnConstants.Price);

            if (headers.AnyIsNullOrEmpty()) return;

            var OldPriceColumn = page.InitialNeedColumn(ColumnConstants.PriceOld);
            if (OldPriceColumn == 0) OldPriceColumn = page.InitialNeedColumn(ColumnConstants.Price);

            decimal priceIncrease = 0;
            if (ProductProcessingOptions.priceIncreasePercentage < 100)
                priceIncrease = (ProductProcessingOptions.priceIncreasePercentage + 100) / 100;
            else
            {
                AssistanceExtensions.Warning(ProductProcessingOptions.priceIncreasePercentage >= 200, $"Are you sure you want a {ProductProcessingOptions.priceIncreasePercentage}% increase?");
                priceIncrease = ProductProcessingOptions.priceIncreasePercentage / 100;
            }

            foreach (var row in worksheet.GetFullRowRangeWithoutFirstRow())
            {
                var price = worksheet.GetDecimal(row, headers.neededColumn);
                if (price == null) continue;
                decimal priceValue = (decimal)price * priceIncrease;

                if (ProductProcessingOptions.ShouldRoundPrices) priceValue = RoundDecimal(priceValue, 0);
                else priceValue = RoundDecimal(priceValue, 2);

                worksheet.WriteCell(row, OldPriceColumn, priceValue);
            }
        }
    }
}
