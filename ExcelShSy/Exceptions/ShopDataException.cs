namespace ExcelShSy.Core.Exceptions
{
    /// <summary>
    /// Represents errors that occur when processing shop-specific data or mappings.
    /// </summary>
    /// <param name="message">A description of the error.</param>
    public class ShopDataException(string message) : AppException(message);
}
