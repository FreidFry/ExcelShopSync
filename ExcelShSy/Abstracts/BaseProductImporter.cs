using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Core.Interfaces.Excel;
using ExcelShSy.Core.Interfaces.Shop;
using ExcelShSy.Core.Interfaces.Storage;

namespace ExcelShSy.Core.Abstracts
{
    public abstract class BaseProductImporter
    {
        protected readonly IProductStorage DataProduct;
        protected readonly IShopStorage ShopStorage;
        protected readonly ILogger Logger;
        protected string ShopName = string.Empty;

        protected BaseProductImporter(IProductStorage dataProduct, IShopStorage shopStorage, ILogger logger )
        {
            DataProduct = dataProduct;
            ShopStorage = shopStorage;
            Logger = logger;
        }

        public void FetchAllProducts(IExcelFile file)
        {
            ShopName = file.ShopName;
            foreach (var page in file.SheetList)
            {
                Logger.Log($"{page.SheetName}");
                ProcessPage(page);
            }
        }

        protected abstract void ProcessPage(IExcelSheet page);
    }
}
