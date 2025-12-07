

using ExcelShSy.Core.Enums;

namespace ExcelShSy.Core.Interfaces.Common
{
    public interface IMessagesService<T>
    {
        T GetMessageBoxStandard(string title, string message);
        T GetMessageBoxStandard(string title, string message, MyButtonEnum buttons);
        T GetMessageBoxStandard(string title, string message, MyButtonEnum buttons, MyIcon icon);
    }
}
