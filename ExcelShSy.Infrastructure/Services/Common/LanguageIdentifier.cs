using ExcelShSy.Core.Interfaces.Common;

namespace ExcelShSy.Infrastructure.Services.Common
{
    public class LanguageIdentifier : ILanguageIdentifier
    {
        private readonly Dictionary<string, HashSet<char>> _languageChars;

        public LanguageIdentifier()
        {
            _languageChars = new Dictionary<string, HashSet<char>>
            {
                ["ru"] = [.. "абвгдеёжзийклмнопрстуфхцчшщъыьэюя"],
                ["uk"] = [.. "абвгґдеєжзиіїйклмнопрстуфхцчшщьюя"],
                ["en"] = [.. "abcdefghijklmnopqrstuvwxyz"]
            };
        }

        public string IdentifyLanguage(string text)
        {
            if (string.IsNullOrEmpty(text))
                return "unknown";

            var lowerText = text.ToLower();

            string bestLang = "unknown";
            int maxCount = 0;

            foreach (var lang in _languageChars)
            {
                int count = lowerText.Count(c => lang.Value.Contains(c));
                if (count > maxCount)
                {
                    maxCount = count;
                    bestLang = lang.Key;
                }
            }

            return bestLang;
        }
    }

}
