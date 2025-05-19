using ExcelShSy.Core.Interfaces;
using ExcelShSy.Infrastracture.Persistance.Model;
using ExcelShSy.Infrastracture.Persistance.ShopData.Datas;

namespace ExcelShSy.Infrastracture.Persistance.ShopData
{
    public class ShopMappings : IShopMappings
    {
        public Dictionary<string, IShopTemplate> Shops => new Dictionary<string, IShopTemplate>
        {
            {ShopNameConstant.Horoshop, new HoroshopData() },
            {ShopNameConstant.Rozetka, new RozetkaData() },
            {ShopNameConstant.Epicenter, new EpicenterData() },
            {ShopNameConstant.Ibud, new IbudData() },
            {ShopNameConstant.Prom, new PromData() }
        };
    }
}
