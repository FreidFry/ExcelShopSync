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
    [Task(nameof(ProductProcessingOptions.ShouldSyncDiscountDate))]
    public class SyncDiscountDate : IExecuteOperation
    {
        private readonly IProductStorage _dataProduct;
        private readonly IFileStorage _fileStorage;
        private readonly IShopStorage _shopMapping;
        private readonly ILogger _logger;
        private readonly IDatabaseSearcher _databaseSearcher;
        private string? _shopFormat;
        
        private string _shopName = string.Empty;
        
        public List<string> Errors { get; } = [];

        public SyncDiscountDate(IProductStorage dataProduct, IFileStorage fileStorage, ILogger logger, IShopStorage shopMapping, IDatabaseSearcher databaseSearcher)
        {
            _dataProduct = dataProduct;
            _fileStorage = fileStorage;
            _logger = logger;
            _databaseSearcher = databaseSearcher;

            _shopMapping = shopMapping;
        }

        public void Execute()
        {
            foreach (var file in _fileStorage.Target)
            {
                _shopFormat = SetDataFormat(file.ShopName);
                _shopName = file.ShopName;
                ProcessFile(file);
            }
        }

        private string SetDataFormat(string shopName)
        {
            var shopTemplate = _shopMapping.GetShopMapping(shopName);
            return shopTemplate.DataFormat;
        }

        private void ProcessFile(IExcelFile file)
        {
            foreach (var page in file.SheetList) OperationWraper.Try(() => ProcessPage(page), Errors, file.FileName);
        }

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
                _logger.Log($"No column {ColumnConstants.DiscountFrom} in {page.SheetName}");
            }
            try
            {
                var endDate = page.InitialNeedColumn(ColumnConstants.DiscountTo);
                dataEnd = endDate;
            }
            catch
            {
                _logger.Log($"No column {ColumnConstants.DiscountTo} in {page.SheetName}");
            }

            if (dataStart == 0 && dataEnd == 0) return;

            var productTo = new List<string>();
            var errorDate = new List<string>();
            foreach (var row in worksheet.GetFullRowRangeWithoutFirstRow())
            {
                var localArticle = worksheet.GetArticle(row, articleCol);
                if (localArticle == null) continue;
                var article = _databaseSearcher.SearchProduct(_shopName, localArticle);

                if (_dataProduct.DiscountFrom.TryGetValue(article, out DateOnly valueFrom))
                    worksheet.WriteCell(row, dataStart, ConvertDate(valueFrom));
                if (_dataProduct.DiscountTo.TryGetValue(article, out DateOnly valueTo))
                    worksheet.WriteCell(row, dataEnd, ConvertDate(valueTo));
                else productTo.Add(article);

                if (valueFrom > ProductProcessingOptions.MinDateActually && valueFrom < valueTo)
                    errorDate.Add(article);
            }

            var products = productTo
                .Union(errorDate)
                .Distinct()
                .ToList();

            _logger.Log($"Products where errors {string.Join(",", products)}");
        }

        private string ConvertDate(DateOnly date)
        {
            return date.ToString(_shopFormat);
        }
    }
}
