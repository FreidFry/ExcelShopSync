using System.Diagnostics.CodeAnalysis;

namespace ExcelShSy.Core.Extensions
{
    public static class ListExtension
    {
        public static bool IsNullOrEmpty([NotNullWhen(false)] this IList<string>? list)
        {
            return list == null || list.Count == 0;
        }
    }
}
