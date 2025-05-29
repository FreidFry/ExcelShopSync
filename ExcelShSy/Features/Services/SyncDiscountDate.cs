using ExcelShSy.Core.Attributes;
using ExcelShSy.Core.Extensions;
using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Core.Interfaces.Excel;
using ExcelShSy.Core.Interfaces.Operations;
using ExcelShSy.Core.Interfaces.Storage;
using ExcelShSy.Core.Services.Logger;
using ExcelShSy.Infrastracture.Persistance.DefaultValues;
using ExcelShSy.Properties;

namespace ExcelShSy.Features.Services
{
    [Task("SyncDiscountDate")]
    public class SyncDiscountDate : IExecuteOperation
    {
        private readonly IDataProduct _dataProduct;
        private readonly IFileStorage _fileStorage;
        private readonly ILogger _logger;

        public SyncDiscountDate(IDataProduct dataProduct, IFileStorage fileStorage, ILogger logger)
        {
            _dataProduct = dataProduct;
            _fileStorage = fileStorage;
            _logger = logger;
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

            var DataStart = page.InitialHeadersTuple(ColumnConstants.DiscountFrom);
            var DataEnd = page.InitialHeadersTuple(ColumnConstants.DiscountTo);

            if (DataStart.AnyIsNullOrEmpty() && DataEnd.AnyIsNullOrEmpty()) return;

            var productTo = new List<string>();
            var ErrorDate = new List<string>();
            foreach (var row in worksheet.GetFullRowRangeWithoutFirstRow())
            {
                var article = worksheet.GetArticle(row, DataStart.articleColumn);

                if (article == null) continue;
                if (_dataProduct.DiscountFrom.TryGetValue(article, out DateOnly valueFrom))
                    worksheet.WriteCell(row, DataStart.neededColumn, valueFrom);
                if (_dataProduct.DiscountTo.TryGetValue(article, out DateOnly valueTo))
                    worksheet.WriteCell(row, DataStart.neededColumn, valueTo);
                else productTo.Add(article);

                if(valueFrom > GlobalSettings.MinDateActually && valueFrom < valueTo)
                    ErrorDate.Add(article);
            }

            var products = productTo
                .Union(ErrorDate)
                .Distinct()
                .ToList();

            _logger.Log($"Products where errors {string.Join(",", products)}");
        }
    }
}
