using System;
using Avalonia.Controls;

namespace ExcelShSy.Event
{
    public static class UpdateTextBlockEvents
    {
        public static event Action<string, string> OnTextUpdate;

        public static void UpdateText(string key, string newText)
        {
            OnTextUpdate?.Invoke(key, newText);
        }
        
        private static void RegestrationTextBlockEvent(string key, TextBlock textBlock)
        {
            UpdateTextBlockEvents.OnTextUpdate += (tarketKey, text) =>
            {
                if (tarketKey == key)
                    textBlock.Text = text;
            };
        }
    }
}
