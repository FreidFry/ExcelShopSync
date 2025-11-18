namespace ExcelShSy.Core.Exceptions
{
    /// <summary>
    /// Serves as the base exception type for application-specific errors.
    /// </summary>
    /// <param name="message">A description of the error.</param>
    public class AppException(string message) : Exception(message);
}
