using ExcelShSy.Core.Attributes;
using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Core.Interfaces.Excel;
using ExcelShSy.Core.Interfaces.Operations;
using ExcelShSy.Core.Interfaces.Shop;
using ExcelShSy.Core.Interfaces.Storage;
using ExcelShSy.Infrastructure.Extensions;
using ExcelShSy.Infrastructure.Persistance.DefaultValues;
using ExcelShSy.Properties;

namespace ExcelShSy.Infrastructure.Services
{
    [Task("SyncDiscountDate")]
    public class SyncDiscountDate : IExecuteOperation
    {
        private readonly IProductStorage _dataProduct;
        private readonly IFileStorage _fileStorage;
        private readonly IShopMapping _shopMapping;
        private readonly ILogger _logger;
        private string _shopFormat;

        public List<string> Errors { get; } = [];

        public SyncDiscountDate(IProductStorage dataProduct, IFileStorage fileStorage, ILogger logger, IShopMapping shopMapping)
        {
            _dataProduct = dataProduct;
            _fileStorage = fileStorage;
            _logger = logger;

            _shopMapping = shopMapping;
        }

        public void Execute()
        {
            foreach (var file in _fileStorage.Target)
            {
                _shopFormat = SetDataFormat(file.ShopName);
                ProcessFile(file);
            }
        }

        string SetDataFormat(string shopName)
        {
            var shopTemplate = _shopMapping.FindShopTemplate(shopName);
            return shopTemplate.DataFormat;
        }

        void ProcessFile(IExcelFile file)
        {
            foreach (var page in file.SheetList) OperationWraper.Try(() => ProcessPage(page), Errors, file.FileName);
        }

        void ProcessPage(IExcelSheet page)
        {
            var worksheet = page.Worksheet;

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
                    worksheet.WriteCell(row, DataStart.neededColumn, ConvertDate(valueFrom));
                if (_dataProduct.DiscountTo.TryGetValue(article, out DateOnly valueTo))
                    worksheet.WriteCell(row, DataStart.neededColumn, ConvertDate(valueTo));
                else productTo.Add(article);

                if (valueFrom > ProductProcessingOptions.MinDateActually && valueFrom < valueTo)
                    ErrorDate.Add(article);
            }

            var products = productTo
                .Union(ErrorDate)
                .Distinct()
                .ToList();

            _logger.Log($"Products where errors {string.Join(",", products)}");
        }

        string ConvertDate(DateOnly date)
        {
            return date.ToString(_shopFormat);
        }
    }
}
