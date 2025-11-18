namespace ExcelShSy.Core.Interfaces.Shop
{
    /// <summary>
    /// Defines a factory that produces shop templates for a specific marketplace.
    /// </summary>
    public interface IShopTemplateFactory
    {
        /// <summary>
        /// Creates a new instance of a shop template.
        /// </summary>
        /// <returns>The instantiated shop template.</returns>
        IShopTemplate Create();
    }
}