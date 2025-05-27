using ExcelShSy.Core.Attributes;
using ExcelShSy.Core.Extensions;
using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Core.Interfaces.Excel;
using ExcelShSy.Core.Interfaces.Operations;
using ExcelShSy.Core.Interfaces.Shop;
using ExcelShSy.Core.Interfaces.Storage;
using ExcelShSy.Infrastracture.Persistance.DefaultValues;
using ExcelShSy.Infrastracture.Persistance.Model;

namespace ExcelShSy.Features.Services
{
    [Task("SyncAvailability")]
    class SyncAvailability : IExecuteOperation
    {
        private readonly IDataProduct _dataProduct;
        private readonly IFileStorage _fileStorage;
        private readonly IShopMappings _shopMapping;

        private string ShopName = ShopNameConstant.Unknown;

        public SyncAvailability(IDataProduct dataProduct, IFileStorage fileStorage, IShopMappings shopMappings)
        {
            _dataProduct = dataProduct;
            _fileStorage = fileStorage;
            _shopMapping = shopMappings;
        }
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
            foreach (var page in file.Pages) ProcessPage(page);
        }

        void ProcessPage(IExcelPage page)
        {
            var shopTemplate = _shopMapping.GetShop(ShopName);
            var worksheet = page.ExcelWorksheet;

            var headers = page.InitialHeadersTuple(ColumnConstants.Availability);

            if (headers.AnyIsNullOrEmpty()) return;

            var product = new List<string>();
            foreach (var row in worksheet.GetFullRowRangeWithoutFirstRow())
            {
                var article = worksheet.GetArticle(row, headers.articleColumn);

                if (article == null) continue;
                if (_dataProduct.Availability.TryGetValue(article, out var value))
                worksheet.WriteCell(row, headers.neededColumn, shopTemplate.Availability[value]);
                else product.Add(article);

            }
        }
    }
}
