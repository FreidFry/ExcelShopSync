using System.ComponentModel;

namespace ExcelShSy.Ui.ModelView.Models.EditLoadFiles
{
    public class ExcelFileItem : INotifyPropertyChanged
    {
        private bool _isChecked;
        private string _fileName;
        private readonly string _filePath;

        #region IsSelectedToRemove

        public bool IsSelectedToRemove
        {
            get => _isChecked;
            set
            {
                if (_isChecked == value) return;
                _isChecked = value;
                OnPropertyChanged(nameof(IsSelectedToRemove));
            }
        }


        #endregion
        
        #region FileName
        public string Name
        {
            get => _fileName;
            set
            {
                if (_fileName == value) return;
                _fileName = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        #endregion
        
        #region FilePath
        public string FilePath
        {
            get => _filePath;
            init
            {
                if (_filePath == value) return;
                _filePath = value;
                OnPropertyChanged(nameof(FilePath));
            }
        }

        #endregion

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public ExcelFileItem(string path)
        {
            Name = Path.GetFileNameWithoutExtension(path);
            IsSelectedToRemove = false;
            FilePath = path;
        }
#if DESIGNER
        public ExcelFileItem()
        { }
#endif
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.


        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public AsyncRelayCommands ShowInfoCommand { get; set; }
    }
}
