namespace ExcelShSy.Core.Interfaces.Excel
{
    /// <summary>
    /// Represents a coordinator responsible for retrieving product data from configured sources.
    /// </summary>
    public interface IGetProductManager
    {
        /// <summary>
        /// Begins fetching products from all configured sources.
        /// </summary>
        void FetchAllProducts();
    }
}
