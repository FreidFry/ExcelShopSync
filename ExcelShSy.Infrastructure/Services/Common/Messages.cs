using ExcelShSy.Core.Enums;
using ExcelShSy.Core.Interfaces.Common;
using MsBox.Avalonia.Base;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia.Models;

namespace ExcelShSy.Infrastructure.Services.Common
{
    public class Messages : IMessagesService<IMsBox<ButtonResult>>, IMessagesCustomService<IMsBox<string>, MessageBoxCustomParams>
    {
        public IMsBox<ButtonResult> GetMessageBoxStandard(string title, string message)
        {
            return MsBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandard(title, message, MapButton(MyButtonEnum.Ok), MapIcon(MyIcon.None));
        }

        public IMsBox<ButtonResult> GetMessageBoxStandard(string title, string message, MyButtonEnum buttons)
        {
            return MsBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandard(title, message, MapButton(buttons), MapIcon(MyIcon.None));
        }

        public IMsBox<ButtonResult> GetMessageBoxStandard(string title, string message, MyButtonEnum buttons, MyIcon icon)
        {
            return MsBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandard(title, message, MapButton(buttons), MapIcon(icon));
        }


        public IMsBox<string> GetMessageBoxCustom(MessageBoxCustomParams CustomParams)
        {
            return MsBox.Avalonia.MessageBoxManager.GetMessageBoxCustom(CustomParams);
        }

        public IMsBox<string> GetMessageBoxCustom(string title, string message, string[] buttons, MyIcon icon = MyIcon.None)
        {
            var customParams = new MessageBoxCustomParams
            {
                ButtonDefinitions = buttons.Select(b => new ButtonDefinition() { Name = b }),
                ContentTitle = title,
                InputParams = new InputParams(),
                ContentMessage = message,
                Icon = MapIcon(icon),
            };
            return MsBox.Avalonia.MessageBoxManager.GetMessageBoxCustom(customParams);
        }

        #region MAPS

        private static ButtonEnum MapButton(MyButtonEnum b)
        {
            return b switch
            {
                MyButtonEnum.Ok => ButtonEnum.Ok,
                MyButtonEnum.OkCancel => ButtonEnum.OkCancel,
                MyButtonEnum.YesNo => ButtonEnum.YesNo,
                MyButtonEnum.YesNoCancel => ButtonEnum.YesNoCancel,
                _ => ButtonEnum.Ok,
            };
        }

        private static Icon MapIcon(MyIcon i)
        {
            return i switch
            {
                MyIcon.None => Icon.None,
                MyIcon.Warning => Icon.Warning,
                MyIcon.Error => Icon.Error,
                MyIcon.Question => Icon.Question,
                _ => Icon.None,
            };
        }
        #endregion
    }
}
