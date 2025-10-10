using ExcelShSy.Core.Interfaces.Shop;
using Microsoft.Extensions.DependencyInjection;

namespace ExcelShSy.Infrastructure.Factories
{
    public class ShopTemplateFactory(IServiceProvider serviceProvider) : IShopTemplateFactory
    {
        public IShopTemplate Create()
        {
            var shopTemplate = serviceProvider.GetRequiredService<IShopTemplate>();
            return shopTemplate;
        }
    }
}
