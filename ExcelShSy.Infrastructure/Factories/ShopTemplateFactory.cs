using ExcelShSy.Core.Interfaces.Shop;
using Microsoft.Extensions.DependencyInjection;

namespace ExcelShSy.Infrastructure.Factories
{
    public class ShopTemplateFactory : IShopTemplateFactory
    {
        private readonly IServiceProvider _serviceProvider;
        public ShopTemplateFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IShopTemplate Create()
        {
            var shopTemplate = _serviceProvider.GetRequiredService<IShopTemplate>();
            return shopTemplate;
        }
    }
}
