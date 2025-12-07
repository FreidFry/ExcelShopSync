using ExcelShSy.Core.Enums;

namespace ExcelShSy.Core.Interfaces.Common
{
    public interface IMessagesCustomService<TOut, TIn>
    {
        TOut GetMessageBoxCustom(TIn CustomParams);
        TOut GetMessageBoxCustom(string title, string message, string[] buttons, MyIcon icon = MyIcon.None);
    }
}
