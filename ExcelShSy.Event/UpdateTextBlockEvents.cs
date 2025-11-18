using System;
using Avalonia.Controls;

namespace ExcelShSy.Event
{
    /// <summary>
    /// Provides event-based helpers for updating text blocks across the UI.
    /// </summary>
    public static class UpdateTextBlockEvents
    {
        /// <summary>
        /// Occurs when a text block registered with <see cref="RegistrationTextBlockEvent"/> should update its text.
        /// </summary>
        public static event Action<string, string>? OnTextUpdate;

        /// <summary>
        /// Raises the <see cref="OnTextUpdate"/> event for the specified key.
        /// </summary>
        /// <param name="key">The identifier of the text block to update.</param>
        /// <param name="newText">The new text to assign.</param>
        public static void UpdateText(string key, string newText)
        {
            OnTextUpdate?.Invoke(key, newText);
        }
        
        /// <summary>
        /// Registers a text block so that it responds to updates triggered via <see cref="UpdateText"/>.
        /// </summary>
        /// <param name="key">The identifier the text block listens for.</param>
        /// <param name="textBlock">The text block instance to update.</param>
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
