namespace ExcelShSy.LocalDataBaseModule.Persistance.DbModel
{
    public class Shop
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<ProductNameMapping> ProductNameMappings { get; set; }
    }
}