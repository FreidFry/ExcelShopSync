using System.Globalization;
using ExcelShSy.Core.Attributes;
using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Core.Interfaces.Excel;
using ExcelShSy.Core.Interfaces.Operations;
using ExcelShSy.Core.Interfaces.Storage;
using ExcelShSy.Core.Properties;
using ExcelShSy.Infrastructure.Extensions;
using ExcelShSy.Infrastructure.Persistence.DefaultValues;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
using static ExcelShSy.Infrastructure.Extensions.ExcelRangeExtensions;
using static ExcelShSy.Localization.GetLocalizationInCode;

namespace ExcelShSy.Infrastructure.Services
{
    [Task(nameof(ProductProcessingOptions.ShouldIncreasePrices))]
    public class IncreasePricePercent : IExecuteOperation
    {
        private readonly IFileStorage _fileStorage;
        private readonly ILocalizationService _localizationService;

        public IncreasePricePercent(IFileStorage fileStorage, ILocalizationService localizationService)
        {
            _fileStorage = fileStorage;
            _localizationService = localizationService;
        }

        public List<string> Errors { get; } = [];

        public void Execute()
        {
            foreach (var file in _fileStorage.Target) ProcessFile(file);
        }

        private void ProcessFile(IExcelFile file)
        {
            foreach (var page in file.SheetList) OperationWraper.Try(() => ProcessPage(page), Errors, file.FileName);
        }

        private async void ProcessPage(IExcelSheet page)
        {

            var worksheet = page.Worksheet;

            var headers = page.InitialHeadersTuple(ColumnConstants.Price);

            if (headers.AnyIsNullOrEmpty()) return;

            var oldPriceColumn = page.InitialNeedColumn(ColumnConstants.PriceOld);
            if (oldPriceColumn == 0) oldPriceColumn = page.InitialNeedColumn(ColumnConstants.Price);

            decimal priceIncrease;
            if (ProductProcessingOptions.priceIncreasePercentage < 100)
                priceIncrease = (ProductProcessingOptions.priceIncreasePercentage + 100) / 100;
            else
            {
                if(ProductProcessingOptions.priceIncreasePercentage >= 200)
                {
                    var title = _localizationService.GetMessageString("Confirm");
                    var msg = _localizationService.GetMessageString("ConfirmText");
                    var formatedText = string.Format(msg, ProductProcessingOptions.priceIncreasePercentage.ToString(CultureInfo.InvariantCulture));
                    
                    var msBox = MessageBoxManager.GetMessageBoxStandard(title,
                        formatedText,
                        ButtonEnum.YesNo);
                    var result = await msBox.ShowAsync();
                    if (result == ButtonResult.No) return;
                }

                priceIncrease = ProductProcessingOptions.priceIncreasePercentage / 100;
            }

            foreach (var row in worksheet.GetFullRowRangeWithoutFirstRow())
            {
                var price = worksheet.GetDecimal(row, headers.neededColumn);
                if (price == null) continue;
                var priceValue = (decimal)price * priceIncrease;

                priceValue = RoundDecimal(priceValue, ProductProcessingOptions.ShouldRoundPrices ? 0 : 2);

                worksheet.WriteCell(row, oldPriceColumn, priceValue);
            }
        }
    }
}
