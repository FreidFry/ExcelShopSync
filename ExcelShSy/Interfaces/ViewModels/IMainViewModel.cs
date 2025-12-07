
namespace ExcelShSy.Core.Interfaces.ViewModels
{
    public interface IMainViewModel
    {
        string IncreasePercentTextBox { get; set; }

        void ShowAboutWindow();
        void ShowDbManager();
        void ShowSettingsWindow();
        void ShowEditLoadFiles();
        Task CheckForUpdates();
        Task ExecuteTasks();
    }
}
