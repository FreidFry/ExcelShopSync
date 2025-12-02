using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;

namespace ExcelShSy.Core.Helpers;

/// <summary>
/// Provides helper methods for working with application windows in Avalonia.
/// </summary>
public static class WindowHelper
{
    /// <summary>
    /// Retrieves the currently active <see cref="Window"/> for the application, if available.
    /// </summary>
    /// <returns>The active window instance, or <c>null</c> when no window is available.</returns>
    public static Window? GetActiveWindow()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            return desktop.MainWindow!;
        return null;
    }
}