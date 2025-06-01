using ExcelShSy.Core.Interfaces.Storage;
using ExcelShSy.Infrastructure.Extensions;

using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Globalization;
using System.IO;

namespace ExcelShSy
{
    public class FileNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string path)
                return Path.GetFileName(path);
            return value;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Interaction logic for EditLoadFilesWindow.xaml
    /// </summary>
    public partial class EditLoadFilesWindow : Window
    {
        public ObservableCollection<string> TemperaryFile { get; set; } = [];
        private readonly IFileManager _fileManager;
        private readonly IFileProvider _fileProvider;
        private bool _firstOpen = true;

        public EditLoadFilesWindow(IFileManager fileManager, IFileProvider fileProvider, string selectedTab)
        {
            InitializeComponent();
            _fileManager = fileManager;
            _fileProvider = fileProvider;

            _firstOpen = true;
            DataContext = this;
            if (selectedTab != null)
                CallMethodForTabFirstOpen(selectedTab);
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement fe && fe.DataContext is string item)
                TemperaryFile.Remove(item);
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl tabControl)
            {
                var tabItem = tabControl.SelectedItem as TabItem;
                if (tabItem != null)
                    CallMethodForTab(tabItem.Name);
            }
        }

        private void CallMethodForTab(string name)
        {
            switch (name)
            {
                case "Target":
                    MethodForTarget();
                    break;
                case "Source":
                    MethodForSource();
                    break;
            }
        }

        private void CallMethodForTabFirstOpen(string name)
        {
            switch (name)
            {
                case "Target":
                    FilesControl.SelectedItem = Target;
                    break;
                case "Source":
                    FilesControl.SelectedItem = Source;
                    break;
            }
        }

        private void AddFromTarget()
        {
            TemperaryFile.Clear();
            foreach (var item in _fileManager.TargetPath)
                TemperaryFile.Add(item);
        }

        private void AddFromSource()
        {
            TemperaryFile.Clear();
            foreach (var item in _fileManager.SourcePath)
                TemperaryFile.Add(item);
        }

        private void MethodForTarget()
        {
            if (!TemperaryFile.IsNullOrEmpty())
            {
                var files = TemperaryFile.ToList();
                if (!_fileManager.SourcePath.SequenceEqual(files) && !_firstOpen)
                {
                    var result = MessageBox.Show("Save changes?", "Save changes", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        _fileManager.SourcePath.Clear();
                        _fileManager.SourcePath.AddRange(files);
                    }
                }
            }
            _firstOpen = false;
            AddFromTarget();
        }

        private void MethodForSource()
        {
            if (!TemperaryFile.IsNullOrEmpty())
            {
                var files = TemperaryFile.ToList();
                if (!_fileManager.TargetPath.SequenceEqual(files) && !_firstOpen)
                {
                    var result = MessageBox.Show("Save changes?", "Save changes", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        _fileManager.TargetPath.Clear();
                        _fileManager.TargetPath.AddRange(files);
                    }
                }
            }
            _firstOpen = false;
            AddFromSource();
        }

        private void AddFiles_Click(object sender, EventArgs e)
        {
            var files = _fileProvider.GetPaths();
            if (files.IsNullOrEmpty()) return;
            foreach (var file in files) if (!TemperaryFile.Contains(file)) TemperaryFile.Add(file);
        }

        private void ShowInfoFile_Click(object sender, EventArgs e)
        {
            if (sender is FrameworkElement fe && fe.DataContext is string item)
            {
                var file = _fileManager.GetFileInfo(item);
                file.ShowInfo();
            }
        }
    }
}
