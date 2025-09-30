using System;
using Avalonia.Controls;

namespace ExcelShSy.Event
{
    public static class UpdateTextBlockEvents
    {
        public static event Action<string, string>? OnTextUpdate;

        public static void UpdateText(string key, string newText)
        {
            OnTextUpdate?.Invoke(key, newText);
        }
        
        public static void RegistrationTextBlockEvent(string key, TextBlock textBlock)
        {
            OnTextUpdate += (targetKey, text) =>
            {
                if (targetKey == key)
                    textBlock.Text = text;
            };
        }
    }
}
