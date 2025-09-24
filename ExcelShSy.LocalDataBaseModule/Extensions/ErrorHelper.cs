using Avalonia.Threading;
using MsBox.Avalonia;

namespace ExcelShSy.LocalDataBaseModule.Extensions;

public static class ErrorHelper
{
    public static void ShowError(string message)
    {
        Dispatcher.UIThread.Post(() =>
        {
            MessageBoxManager
                .GetMessageBoxStandard("Error", message)
                .ShowAsync();
        });
    }

}