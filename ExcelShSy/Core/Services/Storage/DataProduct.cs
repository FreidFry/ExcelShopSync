using ExcelShSy.Core.Interfaces.Common;

using System.Windows;

namespace ExcelShSy.Core.Services.Storage
{
    public class DataProduct : IDataProduct
    {
        public Dictionary<string, decimal> Price { get; set; } = [];
        public Dictionary<string, decimal> Quantity { get; set; } = [];
        public Dictionary<string, string> Availability { get; set; } = [];

        public void AddProductPrice(string productName, decimal price)
        {
            if (Price.ContainsKey(productName))
            {
                MessageBox.Show($"{productName} дублируется цена");
                return;
            }
            Price.Add(productName, price);
        }

        public void AddProductAvailability(string productName, string availability)
        {
            if (Availability.ContainsKey(productName))
            {
                MessageBox.Show($"{productName} дублируется наличие");
                return;
            }
            Availability.Add(productName, availability);
        }


        public void AddProductQuantity(string productName, decimal quantity)
        {
            if (Quantity.ContainsKey(productName))
            {
                MessageBox.Show($"{productName} дублируется количество");
                return;
            }
            Quantity.Add(productName, quantity);
        }
        public void AddProductAvailability(string productName, decimal quantity) => AddProductQuantity(productName, quantity);

        public void ClearAll()
        {
            Price.Clear();
            Quantity.Clear();
            Availability.Clear();
        }
    }
}
