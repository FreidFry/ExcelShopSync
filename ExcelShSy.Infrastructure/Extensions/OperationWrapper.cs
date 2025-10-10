using ExcelShSy.Core.Exceptions;

namespace ExcelShSy.Infrastructure.Extensions
{
    public static class OperationWrapper
    {
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
