using ExcelShSy.Core.Exeptions;

namespace ExcelShSy.Infrastructure.Extensions
{
    public class OperationWraper
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
