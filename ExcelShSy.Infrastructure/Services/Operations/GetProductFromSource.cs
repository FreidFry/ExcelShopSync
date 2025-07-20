using ExcelShSy.Core.Interfaces.Excel;
using ExcelShSy.Core.Interfaces.Operations;
using ExcelShSy.Core.Interfaces.Storage;
using ExcelShSy.Infrastructure.Persistance.Model;

namespace ExcelShSy.Infrastructure.Services.Operations
{
    public class GetProductFromSource : IGetProductManager
    {
        private readonly IFileStorage _fileStorage;

        private readonly IFromPrice _fromPrice;
        private readonly IFromMarket _fromSources;


        public GetProductFromSource(IFileStorage fileStorage, IFromPrice fromPrice, IFromMarket fromSources)
        {
            _fileStorage = fileStorage;

            _fromPrice = fromPrice;
            _fromSources = fromSources;
        }

        public void GetAllProduct()
        {
            foreach (var file in _fileStorage.Source)
            {
                if (file.ShopName == ShopNameConstant.Unknown)
                {
                    _fromPrice.GetAllProduct(file);
                    continue;
                }
                _fromSources.GetAllProduct(file);
            }
        }
    }
}
