using Avalonia.Controls;
using ExcelShSy.Ui.Interfaces;

namespace ExcelShSy.Ui.Services
{
    public class WindowProvider : IWindowProvider
    {
        private readonly List<Window> _windows = [];

        public void RegisterWindow(Window window)
        {
            if (!_windows.Contains(window))
                _windows.Add(window);
        }

        public void UnregisterWindow(Window window)
        {
            if (_windows.Contains(window))
                _windows.Remove(window);
        }

        public Window? GetActiveWindow()
        {
            return _windows.FirstOrDefault(w => w.IsActive);
        }

        public Window? GetWindowByViewModel(object viewModel)
        {
            return _windows.FirstOrDefault(w => w.DataContext == viewModel);
        }

        public Window? GetWindowByType<T>() where T : Window
        {
            return _windows.FirstOrDefault(w => w is T);
        }
    }
}
