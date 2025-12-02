using ExcelShSy.Core.Enums;

namespace ExcelShSy.Core.Interfaces.Common;

/// <summary>
/// Represents user-configurable application settings along with persistence helpers.
/// </summary>
public interface IAppSettings
{
    /// <summary>
    /// Persists the provided settings instance.
    /// </summary>
    /// <param name="settings">The settings to save.</param>
    void SaveSettings(IAppSettings settings);
    
    /// <summary>
    /// Gets or sets the active UI language identifier.
    /// </summary>
    string LanguageCode { get; set; }

    /// <summary>
    /// Gets or sets the active UI language identifier.
    /// </summary>
    LanguaguesEnum.SupportedLanguagues Language { get; set; }

    /// <summary>
    /// Gets or sets the path to the application database file.
    /// </summary>
    string DataBasePath { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether a new file should be created when saving.
    /// </summary>
    bool CreateNewFileWhileSave { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the application should check for updates automatically.
    /// </summary>
    bool AutoCheckUpdate { get; set; }

    /// <summary>
    /// Gets or sets the timestamp of the last update check.
    /// </summary>
    DateTime LastUpdateCheck { get; set; }

    /// <summary>
    /// Occurs when settings change.
    /// </summary>
    event Action? SettingsChanged;
}