using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Core.Interfaces.Operations;
using ExcelShSy.Core.Interfaces.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
