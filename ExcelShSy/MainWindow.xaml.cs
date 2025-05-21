using ExcelShSy.Core.Interfaces.Storage;
using System.Windows;

namespace ExcelShSy
{
    public partial class MainWindow : Window
    {
        private readonly IFileManager _fileManager;

        public MainWindow(IFileManager fileManager)
        {
            InitializeComponent();
            _fileManager = fileManager;
        }

        private void GetTargetFile_Click(object sender, RoutedEventArgs e)
        {
            _fileManager.AddTargetFilesPath(GetTargetFileLable);
        }

        private void GetSourceFile_Click(object sender, RoutedEventArgs e)
        {
            _fileManager.AddSourceFilesPath(GetSourceFileLable);
        }

        private void ExecuteTasks_Click(object sender, RoutedEventArgs e)
        {
            _fileManager.InitializeFiles(FromPriceListCheckBox.IsChecked);
        }
    }
}