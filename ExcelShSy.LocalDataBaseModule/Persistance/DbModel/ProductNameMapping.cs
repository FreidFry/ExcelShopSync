namespace ExcelShSy.LocalDataBaseModule.Persistance.DbModel
{
    public class ProductNameMapping
    {
        public int Id { get; set; }
        public string ExternalName { get; set; }

        public int ProductId { get; set; }
        public int ShopId { get; set; }

        public Product Product { get; set; }
        public Shop Shop { get; set; }
    }
}