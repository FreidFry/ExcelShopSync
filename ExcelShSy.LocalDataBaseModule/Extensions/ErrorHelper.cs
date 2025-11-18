using Avalonia.Threading;
using ExcelShSy.Core.Interfaces.Common;
using MsBox.Avalonia.Base;
using MsBox.Avalonia.Enums;

namespace ExcelShSy.LocalDataBaseModule.Extensions;

public class ErrorHelper(IMessages<IMsBox<ButtonResult>> messages)
{
    public void ShowError(string message)
    {
        Dispatcher.UIThread.Post(() =>
        {
            messages
                .GetMessageBoxStandard("Error", message)
                .ShowAsync();
        });
    }

}