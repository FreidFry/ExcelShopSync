using System.Diagnostics.CodeAnalysis;

namespace ExcelShSy.Core.Extensions
{
    public static class ListExtension
    {
        public static bool IsNullOrEmpty([NotNullWhen(false)] this List<string>? list)
        {
            return list == null || list.Count == 0;
        }
    }
}
