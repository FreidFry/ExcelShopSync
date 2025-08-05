namespace ExcelShSy.Core.Interfaces.Storage
{
    public interface IProductStorage
    {
        List<string> Articles { get; set; }
        Dictionary<string, decimal> Price { get; set; }
        Dictionary<string, decimal> Quantity { get; set; }
        Dictionary<string, string> Availability { get; set; }
        Dictionary<string, decimal> Discount { get; set; }
        Dictionary<string, DateOnly> DiscountFrom { get; set; }
        Dictionary<string, DateOnly> DiscountTo { get; set; }


        void AddProductPrice(string productName, decimal price);
        void AddProductAvailability(string productName, string availability);
        void AddProductQuantity(string productName, decimal quantity);
        void AddProductDiscount(string productName, decimal discount);
        void AddProductDiscountFrom(string productName, DateOnly discount);
        void AddProductDiscountTo(string productName, DateOnly discount);
        void AddProductArticle(string article);
        void ClearData();


    }
}
