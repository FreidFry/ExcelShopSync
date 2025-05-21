using System.Windows.Controls;

namespace ExcelShSy.Core.Interfaces.Storage
{
    public interface IFileManager
    {
        List<string> TargetPath { get; set; }
        List<string> SourcePath { get; set; }

        void AddSourceFiles();
        void AddTargetFiles();

        void InitializeFiles(bool? isPrice);

        void AddSourceFilesPath(Label label);
        void AddTargetFilesPath(Label label);

        void RemoveSourceFilesPath(string path);
        void RemoveTargetFilesPath(string path);
    }
}