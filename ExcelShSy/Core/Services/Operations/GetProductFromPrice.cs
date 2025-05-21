using ExcelShSy.Core.Extensions;
using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Core.Interfaces.Operations;
using ExcelShSy.Core.Interfaces.Storage;
using ExcelShSy.Infrastracture.Persistance.ShopData;

namespace ExcelShSy.Core.Services.Operations
{
    public class GetProductFromPrice : IGetProductFromPrice
    {
        readonly IDataProduct _dataProduct;
        readonly IFileStorage _fileStorage;

        public GetProductFromPrice(IDataProduct dataProduct, IFileStorage fileStorage)
        {
            _dataProduct = dataProduct;
            _fileStorage = fileStorage;
        }

        public void GetProducts()
        {
            foreach (var file in _fileStorage.Source)
                foreach (var page in file.Pages)
                {
                    var worksheet = page.ExcelWorksheet;
                    bool isNull = true;

                    var priceTemplate = ColumnMappingPriceList.Template;
                    foreach (var row in page.GetFullRowRange())
                    {
                        var range = worksheet.GetRowValueColumnMap(row);
                        if (range == null) continue;

                        
                    }
                }
        }
    }
}
