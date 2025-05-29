using ExcelShSy.Core.Attributes;
using ExcelShSy.Core.Extensions;
using ExcelShSy.Core.Interfaces.Excel;
using ExcelShSy.Core.Interfaces.Operations;
using ExcelShSy.Core.Interfaces.Storage;
using ExcelShSy.Infrastracture.Persistance.DefaultValues;
using ExcelShSy.Properties;

using static ExcelShSy.Core.Extensions.ExcelRangeExtensions;

namespace ExcelShSy.Features.Services
{
    [Task("IncreasePricePercent")]
    public class IncreasePricePercent : IExecuteOperation
    {
        private readonly IFileStorage _fileStorage;

        public IncreasePricePercent(IFileStorage fileStorage)
        {
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

            var headers = page.InitialHeadersTuple(ColumnConstants.PriceOld);

            if (headers.AnyIsNullOrEmpty()) headers = page.InitialHeadersTuple(ColumnConstants.Price);
            if (headers.AnyIsNullOrEmpty()) return;

            decimal priceIncrease = 0;
            if (GlobalSettings.priceIncreasePercentage < 100)
                priceIncrease = (GlobalSettings.priceIncreasePercentage + 100) / 100;
            else
            {
                AssistanceExtensions.Warning(GlobalSettings.priceIncreasePercentage >= 200, $"Are you sure you want a {GlobalSettings.priceIncreasePercentage}% increase?");
                priceIncrease /= 100;
            }

            foreach (var row in worksheet.GetFullRowRangeWithoutFirstRow())
            {
                var price = worksheet.GetDecimal(row, headers.neededColumn);
                if (price == null) continue;
                decimal priceValue = (decimal)price * priceIncrease;

                if (GlobalSettings.IsRound) priceValue = RoundDecimal(priceValue, 0);
                else priceValue = RoundDecimal(priceValue, 2);

                worksheet.WriteCell(row, headers.neededColumn, priceValue);
            }
        }
    }
}
