namespace ExcelShSy.Core.Interfaces.Common;

public interface IAppSettings
{
    void SaveSettings(IAppSettings settings);
    
    string Language { get; set; }
    string DataBasePath { get; set; }
    bool CreateNewFileWhileSave { get; set; }
    bool CheckForUpdates { get; set; }
    DateTime LastUpdateCheck { get; set; }
    event Action? SettingsChanged;
}