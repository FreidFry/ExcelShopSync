using ExcelShSy.Core.Enums;
using ExcelShSy.Core.Interfaces.Common;
using MsBox.Avalonia.Base;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;

namespace ExcelShSy.Infrastructure.Services.Common
{
    public class Messages : IMessages<IMsBox<ButtonResult>>, IMessageCustom<IMsBox<string>, MessageBoxCustomParams>
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

        #region MAPS

        private ButtonEnum MapButton(MyButtonEnum b)
        {
            switch (b)
            {
                case MyButtonEnum.Ok:
                    return ButtonEnum.Ok;
                case MyButtonEnum.OkCancel:
                    return ButtonEnum.OkCancel;
                case MyButtonEnum.YesNo:
                    return ButtonEnum.YesNo;
                case MyButtonEnum.YesNoCancel:
                    return ButtonEnum.YesNoCancel;
                default:
                    return ButtonEnum.Ok;
            }
        }

        private Icon MapIcon(MyIcon i)
        {
            switch (i)
            {
                case MyIcon.None:
                    return Icon.None;
                case MyIcon.Warning:
                    return Icon.Warning;
                case MyIcon.Error:
                    return Icon.Error;
                case MyIcon.Question:
                    return Icon.Question;
                default:
                    return Icon.None;
            }
        }
        #endregion
    }
}
