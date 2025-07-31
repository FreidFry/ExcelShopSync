using System.ComponentModel;
using System.IO;

namespace ExcelShSy.Ui.Models.EditLoadFiles
{
    public class FileItem : INotifyPropertyChanged
    {
        private bool _isChecked;
        private string _fileName;
        private string _filePath;

        public bool IsSelectedToRemove
        {
            get => _isChecked;
            set
            {
                if (_isChecked != value)
                {
                    _isChecked = value;
                    OnPropertyChanged(nameof(IsSelectedToRemove));
                }
            }
        }

        public string Name
        {
            get => _fileName;
            set
            {
                if (_fileName != value)
                {
                    _fileName = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public string FilePath
        {
            get => _filePath;
            set { 
            if( _filePath != value)
                {
                    _filePath = value;
                    OnPropertyChanged(nameof(FilePath));
                }
            }
        }

        public FileItem(string path)
        {
            Name = Path.GetFileNameWithoutExtension(path);
            IsSelectedToRemove = false;
            FilePath = path;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
