using ExcelShSy.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelShSy.Core.Services
{
    public class DataProduct : IDataProduct
    {
        public Dictionary<string, decimal> Price { get; set; } = [];
        public Dictionary<string, decimal> Availability { get; set; } = [];

        public void AddProductAvailability(string productName, decimal count)
        {
            if (Price.ContainsKey(productName)) return;
            Price.Add(productName, count);
        }

        public void AddProductPrice(string productName, decimal price)
        {
            if (Availability.ContainsKey(productName)) return;
            Availability.Add(productName, price);
        }
    }
}
