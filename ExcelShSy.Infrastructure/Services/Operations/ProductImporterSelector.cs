using ExcelShSy.Core.Interfaces.Excel;
using ExcelShSy.Core.Interfaces.Operations;
using ExcelShSy.Core.Interfaces.Storage;

namespace ExcelShSy.Infrastructure.Services.Operations
{
    public class ProductImporterSelector(
        IFileStorage fileStorage,
        IFetchPriceListProduct fromPrice,
        IFetchMarketProduct fromSources)
        : IGetProductManager
    {
        public void FetchAllProducts()
        {
            foreach (var file in fileStorage.Source)
            {
                if (file.ShopName == string.Empty)
                {
                    fromPrice.FetchAllProducts(file);
                    continue;
                }
                fromSources.FetchAllProducts(file);
            }
        }
    }
}
