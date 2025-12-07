using Avalonia.Controls;

namespace ExcelShSy.Ui.Interfaces
{
    public interface IWindowProvider
    {
        Window? GetActiveWindow();
        Window? GetWindowByType<T>() where T : Window;
        Window? GetWindowByViewModel(object viewModel);
        void RegisterWindow(Window window);
        void UnregisterWindow(Window window);
    }
}