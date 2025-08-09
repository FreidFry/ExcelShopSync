using ExcelShSy.Core.Attributes;
using ExcelShSy.Core.Interfaces.Excel;
using ExcelShSy.Core.Interfaces.Operations;
using ExcelShSy.Core.Interfaces.Shop;
using ExcelShSy.Core.Interfaces.Storage;
using ExcelShSy.Infrastructure.Extensions;
using ExcelShSy.Infrastructure.Persistance.DefaultValues;
using ExcelShSy.Properties;

namespace ExcelShSy.Infrastructure.Services
{
    [Task(nameof(ProductProcessingOptions.ShouldSyncAvailability))]
    public class SyncAvailability : IExecuteOperation
    {
        private readonly IProductStorage _dataProduct;
        private readonly IFileStorage _fileStorage;
        private readonly IShopStorage _shopMapping;

        private string ShopName = string.Empty;

        public SyncAvailability(IProductStorage dataProduct, IFileStorage fileStorage, IShopStorage shopMappings)
        {
            _dataProduct = dataProduct;
            _fileStorage = fileStorage;
            _shopMapping = shopMappings;
        }

        public List<string> Errors { get; } = [];

        public void Execute()
        {
            foreach (var file in _fileStorage.Target)
            {
                ShopName = file.ShopName;
                ProcessFile(file);
            }
        }

        void ProcessFile(IExcelFile file)
        {
            foreach (var page in file.SheetList) OperationWraper.Try(() => ProcessPage(page), Errors, file.FileName);
        }

        void ProcessPage(IExcelSheet page)
        {
            var shopTemplate = _shopMapping.GetShopMapping(ShopName);
            var worksheet = page.Worksheet;

            var headers = page.InitialHeadersTuple(ColumnConstants.Availability);

            if (headers.AnyIsNullOrEmpty()) return;

            var product = new List<string>();
            foreach (var row in worksheet.GetFullRowRangeWithoutFirstRow())
            {
                var article = worksheet.GetArticle(row, headers.articleColumn);

                if (article == null) continue;
                if (_dataProduct.Availability.TryGetValue(article, out var value))
                worksheet.WriteCell(row, headers.neededColumn, shopTemplate.AvailabilityMap[value]);
                else product.Add(article);
            }
        }
    }
}
