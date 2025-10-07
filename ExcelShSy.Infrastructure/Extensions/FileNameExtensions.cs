using ExcelShSy.Event;

namespace ExcelShSy.Infrastructure.Extensions
{
    public static class FileNameExtensions
    {
        public static void SetLastPath(string textBlockName, List<string> pathList)
        {
            if (pathList.IsNullOrEmpty()) return;
            
            var fileName = GetCleanFileName(pathList);
                
            if (pathList.Count == 1)
                UpdateTextBlockEvents.UpdateText(textBlockName, fileName);
            else
            {
                var normalizedFileName = $"[+ {pathList.Count - 1}] {fileName}";
                UpdateTextBlockEvents.UpdateText(textBlockName, normalizedFileName);
            }
        }

        private static string GetCleanFileName(List<string> pathList) => Path.GetFileNameWithoutExtension(pathList.Last());
    }
}
