using ExcelShSy.Event;

namespace ExcelShSy.Infrastructure.Extensions
{
    /// <summary>
    /// Provides helpers for displaying file path information in the UI.
    /// </summary>
    public static class FileNameExtensions
    {
        /// <summary>
        /// Updates a text block with the most recent file name, including a count of additional files when applicable.
        /// </summary>
        /// <param name="textBlockName">The text block identifier.</param>
        /// <param name="pathList">The list of file paths.</param>
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

        /// <summary>
        /// Retrieves the last file name without its extension from the provided list.
        /// </summary>
        /// <param name="pathList">The list of file paths.</param>
        /// <returns>The normalized file name.</returns>
        private static string GetCleanFileName(List<string> pathList) => Path.GetFileNameWithoutExtension(pathList.Last());
    }
}
