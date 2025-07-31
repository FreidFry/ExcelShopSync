namespace ExcelShSy.Infrastructure.Events
{
    public static class TextBlockEvents
    {
        public static event Action<string, string> OnTextUpdate;

        public static void UpdateText(string key, string newText)
        {
            OnTextUpdate?.Invoke(key, newText);
        }
    }
}
