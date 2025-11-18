namespace ExcelShSy.Core.Interfaces.Common;

/// <summary>
/// Provides localized strings for application messages, errors, and labels.
/// </summary>
public interface ILocalizationService
{
    /// <summary>
    /// Retrieves a localized string from the specified resource file.
    /// </summary>
    /// <param name="resourceFile">The resource file identifier.</param>
    /// <param name="key">The key of the string to retrieve.</param>
    /// <returns>The localized string.</returns>
    string GetString(string resourceFile, string key);
    
    /// <summary>
    /// Retrieves a localized error message by key.
    /// </summary>
    /// <param name="key">The key of the error message.</param>
    /// <returns>The localized error string.</returns>
    string GetErrorString(string key);

    /// <summary>
    /// Retrieves a localized informational message by key.
    /// </summary>
    /// <param name="key">The key of the informational message.</param>
    /// <returns>The localized message string.</returns>
    string GetMessageString(string key);
}