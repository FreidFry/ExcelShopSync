using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Core.Interfaces.Excel;
using ExcelShSy.Core.Interfaces.Shop;
using ExcelShSy.Core.Interfaces.Storage;

namespace ExcelShSy.Core.Abstracts
{
    public abstract class BaseProductImporter
    {
        protected readonly IProductStorage _dataProduct;
        protected readonly IFileStorage _fileStorage;
        protected readonly IShopStorage _shopStorage;
        protected readonly ILogger _logger;
        protected string shopName = string.Empty;

        protected BaseProductImporter(IProductStorage dataProduct, IFileStorage fileStorage, IShopStorage shopStorage, ILogger logger )
        {
            _dataProduct = dataProduct;
            _fileStorage = fileStorage;
            _shopStorage = shopStorage;
            _logger = logger;
        }

        public void FetchAllProducts(IExcelFile file)
        {
            shopName = file.ShopName;
            foreach (var page in file.SheetList)
            {
                _logger.Log($"{page.SheetName}");
                ProcessPage(page);
            }
        }

        protected abstract void ProcessPage(IExcelSheet page);
    }
}
