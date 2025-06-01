using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Core.Interfaces.Excel;
using ExcelShSy.Core.Interfaces.Storage;

namespace ExcelShSy.Core.Abstracts
{
    public abstract class GetProductFromBase
    {
        protected readonly IDataProduct _dataProduct;
        protected readonly IFileStorage _fileStorage;
        protected readonly ILogger _logger;
        protected string shopName = string.Empty;

        protected GetProductFromBase(IDataProduct dataProduct, IFileStorage fileStorage, ILogger logger)
        {
            _dataProduct = dataProduct;
            _fileStorage = fileStorage;
            _logger = logger;            
        }

        public void GetAllProduct(IExcelFile file)
        {
            shopName = file.ShopName;
            foreach (var page in file.Pages)
            {
                _logger.Log($"{page.PageName}");
                ProcessPage(page);
            }
        }

        protected abstract void ProcessPage(IExcelPage page);
    }
}
