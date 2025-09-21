using Avalonia.Threading;
using MsBox.Avalonia;

namespace ExcelShSy.LocalDataBaseModule.Extensions;

public class ErrorHelper
{
    public static void ShowError(string message)
    {
        Dispatcher.UIThread.Post(async () =>
        {
            await MessageBoxManager
                .GetMessageBoxStandard("Error", message)
                .ShowAsync();
        });
    }

}