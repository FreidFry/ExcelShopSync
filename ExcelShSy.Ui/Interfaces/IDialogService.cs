using ExcelShSy.Core.Enums;

namespace ExcelShSy.Ui.Interfaces
{
    public interface IDialogService
    {
        Task<T> RequiredReturnValueDialogAsync<T>(string title, string message, string[] buttons, MyIcon icon = MyIcon.None);
        Task<bool> QuestionDialogAsync(string title, string message, MyIcon icon = MyIcon.Question);
        Task ShowDefaultDialogAsync(string title, string message, MyIcon icon = MyIcon.None);
    }
}