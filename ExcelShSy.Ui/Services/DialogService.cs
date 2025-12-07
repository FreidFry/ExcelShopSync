using ExcelShSy.Core.Enums;
using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Ui.Interfaces;
using MsBox.Avalonia.Base;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;

namespace ExcelShSy.Ui.Services
{
    public class DialogService : IDialogService
    {
        private readonly IWindowProvider _windowProvider;
        private readonly IMessagesService<IMsBox<ButtonResult>> _messageService;
        private readonly IMessagesCustomService<IMsBox<string>, MessageBoxCustomParams> _messageCustom;
        public DialogService(IWindowProvider windowProvider, IMessagesService<IMsBox<ButtonResult>> messagesService, IMessagesCustomService<IMsBox<string>, MessageBoxCustomParams> messageCustom)
        {
            _windowProvider = windowProvider;
            _messageService = messagesService;
            _messageCustom = messageCustom;
        }

        public async Task<T> RequiredReturnValueDialogAsync<T>(string title, string message, string[] buttons, MyIcon icon = MyIcon.None)
        {
            var activeWindow = _windowProvider.GetActiveWindow();

            string? userAction, renamedShop;
            message += "\n";
            do
            {
                var msBox = _messageCustom.GetMessageBoxCustom(title, message, buttons, icon);
                if (activeWindow != null) userAction = await msBox.ShowWindowDialogAsync(activeWindow);
                else userAction = await msBox.ShowAsync();
                renamedShop = NormalizeShopName(msBox.InputValue);
                if ((userAction == buttons.Last() && buttons.Length > 1) || userAction == null!) break;
            }
            while (string.IsNullOrWhiteSpace(renamedShop));
            if (userAction == buttons.Last() && buttons.Length > 1) throw new OperationCanceledException();

            return (T)Convert.ChangeType(renamedShop, typeof(T));
        }
        private static string? NormalizeShopName(string? shopName) => shopName?.Trim().Replace(" ", "_").ToUpper();
        
        public async Task<bool> QuestionDialogAsync(string title, string message, MyIcon icon = MyIcon.Question)
        {
            ButtonResult userAction;
            var activeWindow = _windowProvider.GetActiveWindow();

            var msBox = _messageService.GetMessageBoxStandard(title, message, MyButtonEnum.YesNo, MyIcon.Question);
            if (activeWindow != null)
                userAction = await msBox.ShowWindowDialogAsync(activeWindow);
            else
                userAction = await msBox.ShowAsync();
            return userAction == ButtonResult.Yes;
        }

        public Task ShowDefaultDialogAsync(string title, string message, MyIcon icon = MyIcon.None)
        {
            var activeWindow = _windowProvider.GetActiveWindow();
            var msBox = _messageService.GetMessageBoxStandard(title, message, MyButtonEnum.Ok, icon);
            if (activeWindow != null)
                return msBox.ShowWindowDialogAsync(activeWindow);
            else
                return msBox.ShowAsync();
        }
    }
}
