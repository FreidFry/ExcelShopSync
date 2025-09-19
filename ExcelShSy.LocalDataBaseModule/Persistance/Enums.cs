namespace ExcelShSy.LocalDataBaseModule.Persistance;

internal static class Enums
{
    internal enum Tables
    {
        MasterProducts,
        Shops,
        ProductShopMapping
    }
    
    internal enum CommonColumns
    {
        Id
    }

    internal enum MasterProductsColumns
    {
        MasterArticle
    }

    internal enum ShopsColumns
    {
        Name
    }

    internal enum ProductsMappingColumns
    {
        MasterProductId,
        ShopId,
        Article
    }
}