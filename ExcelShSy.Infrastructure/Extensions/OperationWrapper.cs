using ExcelShSy.Core.Exceptions;

namespace ExcelShSy.Infrastructure.Extensions
{
    /// <summary>
    /// Provides helper methods for executing operations while collecting errors.
    /// </summary>
    public static class OperationWrapper
    {
        /// <summary>
        /// Executes the provided action and captures any <see cref="ShopDataException"/> or general exceptions.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <param name="errors">The error collection to append messages to.</param>
        /// <param name="context">Optional context information to include in error messages.</param>
        public static void Try(Action action, List<string> errors, string context = "")
        {
            try
            {
                action();
            }
            catch (ShopDataException ex)
            {
                var declaringType = action.Method.DeclaringType?.FullName?.Split("+")[0].Split(".").Last();
                errors.Add($"[{context}] {ex.Message} method: \"{declaringType}\"");
            }
            catch (Exception ex)
            {
                errors.Add($"[{context}] Неизвестная ошибка: {ex.Message}");
            }
        }
    }
}
