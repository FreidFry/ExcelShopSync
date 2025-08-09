using ExcelShSy.Core.Interfaces.Excel;
using ExcelShSy.Core.Interfaces.Operations;
using ExcelShSy.Core.Interfaces.Storage;
using ExcelShSy.Infrastructure.Persistance.Model;

namespace ExcelShSy.Infrastructure.Services.Operations
{
    public class ProductImporterSelector : IGetProductManager
    {
        private readonly IFileStorage _fileStorage;

        private readonly IFetchMasterProduct _fromPrice;
        private readonly IFetchMarketProduct _fromSources;


        public ProductImporterSelector(IFileStorage fileStorage, IFetchMasterProduct fromPrice, IFetchMarketProduct fromSources)
        {
            _fileStorage = fileStorage;

            _fromPrice = fromPrice;
            _fromSources = fromSources;
        }

        public void FetchAllProducts()
        {
            foreach (var file in _fileStorage.Source)
            {
                if (file.ShopName == string.Empty)
                {
                    _fromPrice.FetchAllProducts(file);
                    continue;
                }
                _fromSources.FetchAllProducts(file);
            }
        }
    }
}
