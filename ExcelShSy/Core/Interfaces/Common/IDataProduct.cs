namespace ExcelShSy.Core.Interfaces.Common
{
    public interface IDataProduct
    {
        Dictionary<string, decimal> Price { get; set; }
        Dictionary<string, decimal> Quantity { get; set; }

        void AddProductPrice(string productName, decimal price);
        void AddProductAvailability(string productName, string availability);
        void AddProductAvailability(string productName, decimal count);
        void AddProductQuantity(string productName, decimal quantity);
        void ClearAll();


    }
}
