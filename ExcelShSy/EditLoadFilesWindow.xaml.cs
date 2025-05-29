using ExcelShSy.Core.Extensions;
using ExcelShSy.Core.Interfaces.Storage;

using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace ExcelShSy
{
    /// <summary>
    /// Interaction logic for EditLoadFilesWindow.xaml
    /// </summary>
    public partial class EditLoadFilesWindow : Window
    {
        public ObservableCollection<string> TemperaryFile { get; set; } = [];
        private readonly IFileManager _fileManager;
        private readonly IFileProvider _fileProvider;

        public EditLoadFilesWindow(IFileManager fileManager, IFileProvider fileProvider, string selectedTab)
        {
            InitializeComponent();
            _fileManager = fileManager;
            _fileProvider = fileProvider;

            DataContext = this;

            if (selectedTab != null)
                CallMethodForTab(selectedTab);
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement fe && fe.DataContext is string item)
            {
                TemperaryFile.Remove(item);
            }
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
            {
                var tabControl = sender as TabControl;
                var selected = tabControl.SelectedItem as TabItem;

                CallMethodForTab(selected);
                FilesControl.SelectedItem = selected;
            }
        }

        private void CallMethodForTab(TabItem tab)
        {
            CallMethodForTab(tab.Name);
        }

        private void CallMethodForTab(string name)
        {
            switch (name)
            {
                case "Target":
                    MethodForTarget();
                    FilesControl.SelectedItem = Target;
                    break;
                case "Source":
                    MethodForSource();
                    FilesControl.SelectedItem = Source;
                    break;
            }
        }

        private void MethodForTarget()
        {
            if (!TemperaryFile.IsNullOrEmpty())
            {
                var files = TemperaryFile.ToList();
                if (!_fileManager.SourcePath.SequenceEqual(files))
                {
                    var result = MessageBox.Show("Save changes?", "Save changes", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        _fileManager.SourcePath.Clear();
                        _fileManager.SourcePath.AddRange(files);
                    }
                    TemperaryFile.Clear();
                    return;
                }
            }

            TemperaryFile.Clear();
            foreach (var item in _fileManager.TargetPath)
                TemperaryFile.Add(item);
        }

        private void MethodForSource()
        {
            if (!TemperaryFile.IsNullOrEmpty())
            {
                var files = TemperaryFile.ToList();
                if (!_fileManager.TargetPath.SequenceEqual(files))
                {
                    var result = MessageBox.Show("Save changes?", "Save changes", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        _fileManager.TargetPath.Clear();
                        _fileManager.TargetPath.AddRange(files);
                    }
                    TemperaryFile.Clear();
                    return;
                }
            }

            TemperaryFile.Clear();
            foreach (var item in _fileManager.SourcePath)
                TemperaryFile.Add(item);
        }

        private void AddFiles_Click(object sender, EventArgs e)
        {
            var files = _fileProvider.GetPaths();
            if (files.IsNullOrEmpty()) return;
            foreach (var file in files) TemperaryFile.Add(file);
        }
    }
}
