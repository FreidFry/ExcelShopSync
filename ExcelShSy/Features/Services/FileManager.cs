using ExcelShSy.Core.Interfaces;
using ExcelShSy.Features.Interfaces;
using System.IO;
using System.Windows.Controls;

namespace ExcelShSy.Features.Services
{
    public class FileManager : IFileManager
    {
        private readonly IFileProvider _fileProvider;
        private readonly IFileStorage _fileStorage;

        public List<string> TargetPath { get; set; } = [];
        public List<string> SourcePath { get; set; } = [];

        public FileManager(IFileProvider fileProvider, IFileStorage fileStorage)
        {
            _fileProvider = fileProvider;
            _fileStorage = fileStorage;
        }

        public void AddTargetFiles()
        {
            var files = _fileProvider.GetFiles(TargetPath);
            if (files != null)
                _fileStorage.AddTarget(files);
        }

        public void AddSourceFiles()
        {
            var files = _fileProvider.GetFiles(SourcePath);
            if (files != null)
                _fileStorage.AddSource(files);
        }

        public void AddSourceFilesPath(Label label)
        {
            var paths = _fileProvider.GetPaths();

            string LastPath = string.Empty;

            foreach (var path in paths)
                if (File.Exists(path))
                {
                    SourcePath.Add(path);
                    LastPath = path;
                }
            SetLastPath(label, LastPath);
        }

        public void AddTargetFilesPath(Label label)
        {
            var paths = _fileProvider.GetPaths();

            string LastPath = string.Empty;

            foreach (var path in paths)
                if (File.Exists(path))
                {
                    TargetPath.Add(path);
                    LastPath = path;
                }
            SetLastPath(label, LastPath);
        }

        public void RemoveSourceFilesPath(string path)
        {
            if (SourcePath.Contains(path))
                SourcePath.Remove(path);
        }

        public void RemoveTargetFilesPath(string path)
        {
            if (TargetPath.Contains(path))
                TargetPath.Remove(path);
        }

        private static void SetLastPath(Label lable, string path)
        {
            lable.Content = Path.GetFileName(path);
        }
    }
}
