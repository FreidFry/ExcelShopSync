using ExcelShSy.Core.Interfaces.Excel;

using System.Windows.Controls;

namespace ExcelShSy.Core.Interfaces.Storage
{
    public interface IFileManager
    {
        List<string> TargetPath { get; set; }
        List<string> SourcePath { get; set; }

        void AddSourceFiles();
        void AddTargetFiles();

        void InitializeFiles();

        void AddSourceFilesPath();
        void AddTargetFilesPath();

        void RemoveSourceFilesPath(string path);
        void RemoveTargetFilesPath(string path);

        IExcelFile GetFileInfo(string path);
    }
}