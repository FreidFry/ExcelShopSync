using ExcelShSy.Core.Interfaces.Shop;
using ExcelShSy.Infrastructure.Persistance.Model;
using ExcelShSy.Infrastructure.Persistance.ShopData.Datas;

namespace ExcelShSy.Infrastructure.Persistance.ShopData
{
    public class ShopMappings : IShopMappings
    {
        public IShopTemplate GetShop(string shopName) => Shops[shopName];

        public Dictionary<string, IShopTemplate> Shops => new()
        {
            {ShopNameConstant.Horoshop, new HoroshopData() },
            {ShopNameConstant.Rozetka, new RozetkaData() },
            {ShopNameConstant.Epicenter, new EpicenterData() },
            {ShopNameConstant.Ibud, new IbudData() },
            {ShopNameConstant.Prom, new PromData() }
        };
    }
}
