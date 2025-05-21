using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Core.Interfaces.Operations;
using ExcelShSy.Core.Interfaces.Storage;

namespace ExcelShSy.Features.Services
{
    public class SyncPriceWithShop : IExecuteOperation
    {
        public string Name => "SyncPriceWithShop";
        private readonly IDataProduct _dataProduct;
        private readonly IFileStorage _fileStorage;

        public SyncPriceWithShop(IDataProduct dataProduct, IFileStorage fileStorage)
        {
            _dataProduct = dataProduct;
            _fileStorage = fileStorage;

        }


        public void Execute()
        { 
        }
    }
}
