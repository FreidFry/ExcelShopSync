namespace ExcelShSy.Core.Interfaces.Common
{
    public interface IDataProduct
    {
        Dictionary<string, decimal> Price { get; set; }
        Dictionary<string, decimal> Availability { get; set; }

        void AddProductPrice(string productName, decimal price);
        void AddProductAvailability(string productName, decimal count);


    }
}
