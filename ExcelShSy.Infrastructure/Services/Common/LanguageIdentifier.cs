using ExcelShSy.Core.Interfaces.Common;

namespace ExcelShSy.Infrastructure.Services.Common
{
    public class LanguageIdentifier : ILanguageIdentifier
    {
        private readonly Dictionary<string, HashSet<char>> _languageChars = new()
        {
            ["ru"] = [.. "абвгдеёжзийклмнопрстуфхцчшщъыьэюя"],
            ["uk"] = [.. "абвгґдеєжзиіїйклмнопрстуфхцчшщьюя"],
            ["en"] = [.. "abcdefghijklmnopqrstuvwxyz"]
        };

        public string IdentifyLanguage(string text)
        {
            if (string.IsNullOrEmpty(text))
                return "unknown";

            var lowerText = text.ToLower();

            var bestLang = "unknown";
            var maxCount = 0;

            foreach (var lang in _languageChars)
            {
                var count = lowerText.Count(c => lang.Value.Contains(c));
                if (count <= maxCount) continue;
                maxCount = count;
                bestLang = lang.Key;
            }

            return bestLang;
        }
    }

}
