using ExcelShSy.Core.Attributes;
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
    [Task(nameof(ProductProcessingOptions.ShouldSyncAvailability))]
    public class SyncAvailability : IExecuteOperation
    {
        private readonly IProductStorage _dataProduct;
        private readonly IFileStorage _fileStorage;
        private readonly IShopStorage _shopMapping;
        private readonly IDatabaseSearcher _databaseSearcher;

        private string _shopName = string.Empty;

        public SyncAvailability(IProductStorage dataProduct, IFileStorage fileStorage, IShopStorage shopMappings, IDatabaseSearcher databaseSearcher)
        {
            _dataProduct = dataProduct;
            _fileStorage = fileStorage;
            _shopMapping = shopMappings;
            _databaseSearcher = databaseSearcher;
        }

        public List<string> Errors { get; } = [];

        public void Execute()
        {
            foreach (var file in _fileStorage.Target)
            {
                _shopName = file.ShopName;
                ProcessFile(file);
            }
        }

        private void ProcessFile(IExcelFile file)
        {
            foreach (var page in file.SheetList) OperationWraper.Try(() => ProcessPage(page), Errors, file.FileName);
        }

        private void ProcessPage(IExcelSheet page)
        {
            var shopTemplate = _shopMapping.GetShopMapping(_shopName);
            var worksheet = page.Worksheet;

            var headers = page.InitialHeadersTuple(ColumnConstants.Availability);

            if (headers.AnyIsNullOrEmpty()) return;

            foreach (var row in worksheet.GetFullRowRangeWithoutFirstRow())
            {
                var localArticle = worksheet.GetArticle(row, headers.articleColumn);
                if (localArticle == null) continue;
                var article = _databaseSearcher.SearchProduct(_shopName, localArticle);
                
                if (_dataProduct.Availability.TryGetValue(article, out var value))
                    worksheet.WriteCell(row, headers.neededColumn, shopTemplate.AvailabilityMap[value]);
            }
        }
    }
}
