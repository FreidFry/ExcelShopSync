namespace ExcelShSy.LocalDataBaseModule.Persistance.DbModel
{
    public class Product
    {
        public int Id { get; set; }
        public string InternalName { get; set; }

        public ICollection<ProductNameMapping> NameMappings { get; set; }
    }
}