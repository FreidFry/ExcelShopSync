using ExcelShSy.Core.Interfaces.Operations;
using ExcelShSy.Core.Interfaces.Storage;
using System.Windows;

namespace ExcelShSy
{
    public partial class MainWindow : Window
    {
        private readonly IFileManager _fileManager;
        private readonly IGetPricesFromSource _test;

        public MainWindow(IFileManager fileManager, IGetPricesFromSource test)
        {
            InitializeComponent();
            _fileManager = fileManager;
            _test = test;
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
            _fileManager.InitializeFiles();
            _test.GetAllPrice();

        }
    }
}