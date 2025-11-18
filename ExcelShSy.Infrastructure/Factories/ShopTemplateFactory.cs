using ExcelShSy.Core.Interfaces.Shop;
using Microsoft.Extensions.DependencyInjection;

namespace ExcelShSy.Infrastructure.Factories
{
    /// <summary>
    /// Resolves shop template instances from the dependency injection container.
    /// </summary>
    public class ShopTemplateFactory(IServiceProvider serviceProvider) : IShopTemplateFactory
    {
        /// <inheritdoc />
        public IShopTemplate Create()
        {
            var shopTemplate = serviceProvider.GetRequiredService<IShopTemplate>();
            return shopTemplate;
        }
    }
}
