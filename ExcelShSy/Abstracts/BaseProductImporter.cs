using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Core.Interfaces.Excel;
using ExcelShSy.Core.Interfaces.Shop;
using ExcelShSy.Core.Interfaces.Storage;

namespace ExcelShSy.Core.Abstracts
{
    /// <summary>
    /// Provides shared logic for importing products from Excel files while delegating page processing to derived classes.
    /// </summary>
    /// <param name="dataProduct">Storage used to accumulate product data during import.</param>
    /// <param name="shopStorage">Storage that provides shop-specific templates.</param>
    /// <param name="logger">Logger used to report progress and diagnostics.</param>
    public abstract class BaseProductImporter(IProductStorage dataProduct, IShopStorage shopStorage, ILogger logger)
    {
        /// <summary>
        /// Gets the storage used to persist fetched product data during import.
        /// </summary>
        protected readonly IProductStorage DataProduct = dataProduct;

        /// <summary>
        /// Gets the storage that supplies shop templates for mapping columns.
        /// </summary>
        protected readonly IShopStorage ShopStorage = shopStorage;

        /// <summary>
        /// Gets the logger used to emit diagnostic messages during import.
        /// </summary>
        protected readonly ILogger Logger = logger;

        /// <summary>
        /// Gets or sets the template for the current shop, if one is available.
        /// </summary>
        protected IShopTemplate? ShopTemplate;

        /// <summary>
        /// Gets or sets the name of the shop currently being processed.
        /// </summary>
        protected string? ShopName = string.Empty;

        /// <summary>
        /// Fetches products from every sheet within the provided Excel file.
        /// </summary>
        /// <param name="file">The Excel file that contains product data.</param>
        public void FetchAllProducts(IExcelFile file)
        {
            ShopName = file.ShopName;
            if (!string.IsNullOrWhiteSpace(ShopName)) ShopTemplate = ShopStorage.GetShopMapping(ShopName);
            if (file.SheetList == null) return;
            foreach (var page in file.SheetList)
            {
                Logger.Log($"{page.SheetName}");
                ProcessPage(page);
            }
            ShopTemplate = null;
        }

        /// <summary>
        /// Processes a single worksheet page, extracting relevant product data.
        /// </summary>
        /// <param name="page">The worksheet abstraction to process.</param>
        protected abstract void ProcessPage(IExcelSheet page);
    }
}
