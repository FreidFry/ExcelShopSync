using ExcelShSy.Infrastructure.Events;

namespace ExcelShSy.Infrastructure.Extensions
{
    public static class FileNameExtensions
    {
        public static void SetLastPath(string TextBlockName, List<string> pathList)
        {
            if (!pathList.IsNullOrEmpty())
            {
                var fileName = GetCleanFileName(pathList);

                if (fileName.Length > 25)
                    fileName = $"{fileName.Substring(0, 25)}...";
                if (pathList.Count == 1)
                    UpdateTextBlockEvents.UpdateText(TextBlockName, fileName);
                else
                {
                    var normalizedFileName = $"{fileName} [+ {pathList.Count - 1}]";
                    UpdateTextBlockEvents.UpdateText(TextBlockName, normalizedFileName);
                }
            }
        }

        static string GetCleanFileName(List<string> pathList) => Path.GetFileNameWithoutExtension(pathList.Last());
    }
}
