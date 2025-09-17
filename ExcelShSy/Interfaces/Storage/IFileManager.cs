using Avalonia.Controls;
using ExcelShSy.Core.Interfaces.Excel;

namespace ExcelShSy.Core.Interfaces.Storage
{
    public interface IFileManager
    {
        List<string> TargetPaths { get; set; }
        List<string> SourcePaths { get; set; }

        void InitializeSourceFiles();
        void InitializeTargetFiles();

        void InitializeAllFiles();
        void ClearAfterComplete();

        void AddSourcePath();
        void AddTargetPath();

        void RemoveSourcePath(string path);
        void RemoveTargetPath(string path);

        IExcelFile GetFileDetails(string path);
    }
}