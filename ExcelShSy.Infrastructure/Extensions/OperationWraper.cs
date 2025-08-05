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
                errors.Add($"[{context}] {ex.Message}");
            }
            catch (Exception ex)
            {
                errors.Add($"[{context}] Неизвестная ошибка: {ex.Message}");
            }
        }
    }
}
