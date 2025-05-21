namespace ExcelShSy.Core.Extensions
{
    public static class DictionaryExtensions
    {

        public static bool IsNullOrEmpty<TKey, TValue>(this Dictionary<TKey, TValue> dict) 
            where TKey : notnull =>
            dict == null || dict.Count == 0;

        public static bool HasRequiredKeys(this Dictionary<string, int> dict, params string[] keys) =>
            keys.All(key => dict.ContainsKey(key));
    }
}
