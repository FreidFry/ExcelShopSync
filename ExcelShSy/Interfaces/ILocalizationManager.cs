using System.Globalization;

namespace ExcelShSy.Core.Interfaces
{
    /// <summary>
    /// Coordinates the current application culture used for localization purposes.
    /// </summary>
    public interface ILocalizationManager
    {
        /// <summary>
        /// Updates the culture using a culture code (for example, <c>en-US</c>).
        /// </summary>
        /// <param name="code">The culture identifier to use.</param>
        void SetCulture(string code);

        /// <summary>
        /// Updates the culture using the provided <see cref="CultureInfo"/> instance.
        /// </summary>
        /// <param name="culture">The culture information to apply.</param>
        void SetCulture(CultureInfo culture);
    }
}
