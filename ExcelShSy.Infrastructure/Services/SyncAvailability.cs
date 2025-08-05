using ExcelShSy.Core.Attributes;
using ExcelShSy.Core.Interfaces.Excel;
using ExcelShSy.Core.Interfaces.Operations;
using ExcelShSy.Core.Interfaces.Shop;
using ExcelShSy.Core.Interfaces.Storage;
using ExcelShSy.Infrastructure.Extensions;
using ExcelShSy.Infrastructure.Persistance.DefaultValues;
using ExcelShSy.Infrastructure.Persistance.Model;

namespace ExcelShSy.Infrastructure.Services
{
    [Task("SyncAvailability")]
    public class SyncAvailability : IExecuteOperation
    {
        private readonly IProductStorage _dataProduct;
        private readonly IFileStorage _fileStorage;
        private readonly IShopMapping _shopMapping;

        private string ShopName = ShopNameConstant.Unknown;

        public SyncAvailability(IProductStorage dataProduct, IFileStorage fileStorage, IShopMapping shopMappings)
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
            var shopTemplate = _shopMapping.FindShopTemplate(ShopName);
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
