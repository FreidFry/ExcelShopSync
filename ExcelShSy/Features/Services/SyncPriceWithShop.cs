using ExcelShSy.Core.Interfaces;
using ExcelShSy.Features.Interfaces;
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
