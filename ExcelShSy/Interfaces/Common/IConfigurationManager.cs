namespace ExcelShSy.Core.Interfaces.Common;

/// <summary>
/// Provides access to application configuration and settings.
/// </summary>
public interface IConfigurationManager
{
    /// <summary>
    /// Loads application settings from the configured source.
    /// </summary>
    /// <returns>The loaded application settings instance.</returns>
    IAppSettings Load();
}