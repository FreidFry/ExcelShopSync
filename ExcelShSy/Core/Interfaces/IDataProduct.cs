using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelShSy.Core.Interfaces
{
    public interface IDataProduct
    {
        Dictionary<string, decimal> Price { get; set; }
        Dictionary<string, decimal> Availability { get; set; }

        void AddProductPrice(string productName, decimal price);
        void AddProductAvailability(string productName, decimal count);


    }
}
