using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Core.Interfaces.Excel;
using ExcelShSy.Core.Interfaces.Shop;
using ExcelShSy.Core.Interfaces.Storage;

namespace ExcelShSy.Core.Abstracts
{
    public abstract class BaseProductImporter(IProductStorage dataProduct, IShopStorage shopStorage, ILogger logger)
    {
        protected readonly IProductStorage DataProduct = dataProduct;
        protected readonly IShopStorage ShopStorage = shopStorage;
        protected readonly ILogger Logger = logger;
        protected string? ShopName = string.Empty;

        public void FetchAllProducts(IExcelFile file)
        {
            ShopName = file.ShopName;
            if (file.SheetList == null) return;
            foreach (var page in file.SheetList)
            {
                Logger.Log($"{page.SheetName}");
                ProcessPage(page);
            }
        }

        protected abstract void ProcessPage(IExcelSheet page);
    }
}
