namespace ExcelShSy.Core.Interfaces.Common
{
    /// <summary>
    /// Detects the language of provided text fragments.
    /// </summary>
    public interface ILanguageIdentifier
    {
        /// <summary>
        /// Determines the language represented by the specified text.
        /// </summary>
        /// <param name="text">The text whose language should be identified.</param>
        /// <returns>The language code or description.</returns>
        string IdentifyLanguage(string text);
    }
}