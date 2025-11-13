using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using DynamicData;
using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Core.Interfaces.DataBase;
using ExcelShSy.Core.Interfaces.Shop;
using ExcelShSy.Core.Interfaces.Storage;
using ExcelShSy.Infrastructure.Persistence.Model;
using ExcelShSy.LocalDataBaseModule.Persistance;
using Microsoft.Data.Sqlite;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia.Models;
using MsBox.Avalonia.ViewModels.Commands;

namespace ExcelShSy.Ui.Windows
{
    public partial class ShopManagerWindow : Window, INotifyPropertyChanged
    {
        #region Dependency Injections
        
        private readonly IShopStorage _shopStorage;
        private readonly ILogger _logger;
        private readonly IFileProvider _fileProvider;
        private readonly ILocalizationService _localizationService;
        private readonly ISqliteDbContext _sqliteDbContext;
        
        #endregion
        
        #region State

        private bool _shopChanged;
        private bool _ready;
        private bool _shouldShowMessage;
        private int _currentSelection;

        #endregion
        
        #region Fields
                
        #region Current Shop

        private IShopTemplate? _currentShop;
        public IShopTemplate? CurrentShop
        {
            get => _currentShop;
            private set
            {
                if (_currentShop != value)
                {
                    _currentShop = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region Shop Headers

        public ObservableCollection<string?> CurrentShopHeaders { get; set; } = [];

        #endregion
        
        public ObservableCollection<string> MagazineSelectorItems = [];
        
        #endregion

        public new event PropertyChangedEventHandler? PropertyChanged;
        
#if DESIGNER
        public ShopManagerWindow()
        {
            InitializeComponent();
        }
#endif
        
        public ShopManagerWindow(IShopStorage shopStorage, ILogger logger, IFileProvider fileProvider, ILocalizationService localizationService, ISqliteDbContext sqliteDbContext) 
        {
            _shopStorage = shopStorage;
            _logger = logger;
            _fileProvider = fileProvider;
            _localizationService = localizationService;
            _sqliteDbContext = sqliteDbContext;
            
            Initialize();
            
            Loaded += (_, _) => Dispatcher.UIThread.Post(() => _ready = true, DispatcherPriority.Background);
            RegistrationEvents();
        }
        
        #region Initialization

        private void Initialize()
        {
            InitializeComponent();
            InitializeShopSelector();
            DataContext = this;
            Closing += OnClosing;

            AllColumnList.ContextMenu = CreateAllColumnContextMenu(AllColumnList);
            MagazineSelector.ContextMenu = CreateShopSelectorContextMenu(MagazineSelector);
            MagazineSelector.SelectedIndex = 0;
        }

        private void RegistrationEvents()
        {
            _shopStorage.ShopsChanged += OnShopChanged;
        }

        private async void OnClosing(object? sender, WindowClosingEventArgs e)
        {
            if (_shopChanged)
            {
                e.Cancel = true;
                _shopChanged = false;
                SaveShop();
            }

            if (_shouldShowMessage)
            {
                e.Cancel = true;
                var ConfirmClose = await ShowMessage();
                if (ConfirmClose)
                {
                    _shouldShowMessage = false;
                    Close();
                }
            }
        }

        private async Task<bool> ShowMessage()
        {
            var windowName = nameof(ShopManagerWindow);
            var title = _localizationService.GetString(windowName, "ConfirmSaveTitle");
            var msg = _localizationService.GetString(windowName, "ConfirmSaveText");

            var msBox = await MessageBoxManager.GetMessageBoxStandard(title, msg, ButtonEnum.YesNo).ShowWindowDialogAsync(this);
            if (msBox == ButtonResult.Yes) return true;
            return false;
        }

        private void InitializeShopSelector()
        {
            MagazineSelector.Items.Clear();
            foreach(var shop in _shopStorage.GetShopList())
                MagazineSelector.Items.Add(shop);
            var count = MagazineSelector.Items.Count;
            if (count != 0)
                MagazineSelector.SelectedIndex = count - 1;
        }

        private ContextMenu CreateAllColumnContextMenu(ListBox list) => new()
        {
            ItemsSource = new List<MenuItem>
            {
                new()
                {
                    Header = _localizationService.GetString(nameof(ShopManagerWindow), "RemoveButton"),
                    Command = new RelayCommand(_ =>
                    {
                        if (list.SelectedItems?.Count < 1) return;

                        if (list.ItemsSource is ObservableCollection<string> items)
                        {
                            if (list.SelectedItems == null) return;
                            foreach (var selected in list.SelectedItems.Cast<string>().ToList())
                                items.Remove(selected);
                            _shopChanged = true;
                        }
                    })
                }
            }
        };

        private ContextMenu CreateShopSelectorContextMenu(ListBox list) => new()
        {
            ItemsSource = new List<MenuItem>
            {
                new()
                {
                    Header = _localizationService.GetString(nameof(ShopManagerWindow), "RenameButton"),
                    Command = new RelayCommand(_ =>
                    {
                        if(list.SelectedItem is string selected) RenameShopCommand(selected);
                    })
                },
                new()
                {
                    Header = _localizationService.GetString(nameof(ShopManagerWindow), "RemoveButton"),
                    Command = new RelayCommand(_ =>
                    {
                        if (list.SelectedItem is string selected) DeleteShopCommand(selected);
                    })
                }
            }
                
        };

        #endregion
        
        #region Handlers

        #region Events
        private async void SelectMagazine_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (MagazineSelector.SelectedIndex == _currentSelection && _ready) return;
            if (_shopChanged && MagazineSelector.SelectedIndex != _currentSelection)
            {
                const string windowName = nameof(ShopManagerWindow);
                var title = _localizationService.GetString(windowName, "UnsavedChanges");
                var msg = _localizationService.GetString(windowName, "UnsavedChangesText");
                
                var msBox = MessageBoxManager.GetMessageBoxStandard(title, msg, ButtonEnum.YesNoCancel);
                var result = await msBox.ShowWindowDialogAsync(this);
                switch (result)
                {
                    case ButtonResult.Yes:
                        SaveShop();
                        break;
                    case ButtonResult.Cancel:
                        MagazineSelector.SelectedIndex = _currentSelection;
                        return;
                }
            }
            if (MagazineSelector.SelectedItem is string selectedShop)
            {
                _ready = false;
                var shop = _shopStorage.GetShopMapping(selectedShop);
                CurrentShopHeaders.Clear();
                CurrentShopHeaders.Add(null);
                CurrentShopHeaders.AddRange(shop!.UnmappedHeaders);
                CurrentShop = shop;
                _currentSelection = MagazineSelector.SelectedIndex;
            }
            _shopChanged = false;
            Dispatcher.UIThread.Post(() => _ready = true, DispatcherPriority.Background);
        }
        
        private void OnShopChanged(string shopName)
        {
            MagazineSelectorItems.Add(shopName);
            MagazineSelector.SelectedItem = MagazineSelectorItems.Count - 1;
        }
        
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
        
        private void IsChanged_Changed(object sender, RoutedEventArgs e)
        {
            if (_ready)
                _shopChanged = true;
        }
        
        #endregion

        #region Clicks

        private void SaveShopTemplate_Click(object sender, RoutedEventArgs e)
        {
            SaveShop();
        }

        private async void AddNewShop_OnClick(object? sender, RoutedEventArgs e)
        {
            string? shopName;
            string? userAction;
            var windowName = nameof(ShopManagerWindow);
            var createButton = _localizationService.GetString(windowName, "CreateButton");
            var cancelButton = _localizationService.GetString(windowName, "CancelButton");
            var title = _localizationService.GetString(windowName, "CreateNewShopTitle");
            var msg = _localizationService.GetString(windowName, "CreateNewShopText");
            
            do
            {
                var msBox = MessageBoxManager.GetMessageBoxCustom(new MessageBoxCustomParams
                {
                    ButtonDefinitions =
                    [
                        new ButtonDefinition { Name = createButton },
                        new ButtonDefinition { Name = cancelButton }
                    ],
                    ContentTitle = title,
                    ContentMessage = msg,
                    InputParams = new InputParams(),
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    CanResize = false,
                    MaxWidth = 800,
                    MaxHeight = 300,
                    SizeToContent = SizeToContent.WidthAndHeight
                });
                userAction = await msBox.ShowWindowDialogAsync(this);
                //make it global when creating a store and everything related to it
                //now realized in ShopTemplate class
                //                             VVVVVVVVVVVVVVVVVVVVVVVVVVV
                shopName = msBox.InputValue.Trim().Replace(" ", "_").ToUpper();
                if (userAction == cancelButton || userAction == null!) break;
            } 
            while (string.IsNullOrWhiteSpace(shopName));
            if (userAction != createButton) return;
            if (_shopStorage.IsFileNotExist(shopName)) return;
            var shop = new ShopTemplate { Name = shopName };

            _shopStorage.SaveShopTemplate(shop);
            _shopStorage.AddShop(shop.Name);
            InitializeShopSelector();
        }
        
        private async void GetFirstLineFromFile_Click(object? sender, RoutedEventArgs e)
        {
            var path = await _fileProvider.PickExcelFilePath();
            if (path == null) return;
            var file = _fileProvider.FetchExcelFile(path);

            var worksheet = file.SheetList?[0].Worksheet;
            if (worksheet == null) return;
            
            var dimension = worksheet.Dimension;
            var firstRow = dimension.Start.Row;
            var firstColumn = dimension.Start.Column;
            var lastColumn = dimension.End.Column;
            
            var row = worksheet.Cells[firstRow, firstColumn, firstRow, lastColumn];
            
            var values = row.Where(cell => !string.IsNullOrWhiteSpace(cell.Value?.ToString()))
                .Select(cell => cell.Value?.ToString())
                .ToList();

            CurrentShopHeaders.Clear();
            CurrentShopHeaders.AddRange(values);
            _shopChanged = true;
        }
        
        #endregion

        private void SaveShop()
        {
            if (CurrentShop == null)
            {
                if (MagazineSelector.SelectedItem != null)
                {
                    var msg = $"Shop save error shop name: {MagazineSelector.SelectedItem} ";
                    _logger.LogError(msg);
                }

                return;
            }

            var list = CurrentShopHeaders.ToList();
            while (list.Remove(null)) ;
            CurrentShop.UnmappedHeaders = list!;
            _shopStorage.UpdateShop(CurrentShop);
            _shopStorage.SaveShopTemplate(CurrentShop);
            _sqliteDbContext.CreateColumn(CurrentShop.Name);
            _shopChanged = false;
            _shouldShowMessage = true;
        }

        private async void RenameShopCommand(string oldName)
        {
            string? userAction;
            var windowName = nameof(ShopManagerWindow);
            var renameButton = _localizationService.GetString(windowName, "RenameButton");
            var cancelButton = _localizationService.GetString(windowName, "CancelButton");
            var title = _localizationService.GetString(windowName, "RenameShopTitle");
            var msg = _localizationService.GetString(windowName, "RenameShopText");
            var formattedMsg = string.Format(msg, oldName);
            
            string? renamedShop;
            do
            {
                var msBox = MessageBoxManager.GetMessageBoxCustom(new MessageBoxCustomParams
                {
                    ButtonDefinitions =
                    [
                        new ButtonDefinition { Name = renameButton },
                        new ButtonDefinition { Name = cancelButton }
                    ],
                    ContentTitle = title,
                    ContentMessage = formattedMsg,
                    InputParams = new InputParams(),
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    CanResize = false,
                    MaxWidth = 800,
                    MaxHeight = 300,
                    SizeToContent = SizeToContent.WidthAndHeight
                });
                userAction = await msBox.ShowWindowDialogAsync(this);
                //make it global when creating a store and everything related to it
                //now realized in ShopTemplate class
                //                             VVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVV
                renamedShop = msBox.InputValue.Trim().Replace(" ", "_").ToUpper();
                if (userAction == cancelButton || userAction == null!) break;
            }
            while (string.IsNullOrWhiteSpace(renamedShop));
            
            if (userAction != renameButton) return;
            _currentShop!.Name = renamedShop;
            _shopStorage.RenameShop(oldName, renamedShop);
            try
            {
                _sqliteDbContext.RenameColumn($"{Enums.Tables.ProductShopMapping}", oldName, renamedShop);
            }
            catch (SqliteException ex) when (ex.SqliteErrorCode == 1)
            { }
            InitializeShopSelector();
            MagazineSelector.SelectedItem = _shopStorage.Shops.FindIndex(x => x.Name == renamedShop);
        }
        
        private async void DeleteShopCommand(string selected)
        {
            const string windowName = nameof(ShopManagerWindow);
            var title = _localizationService.GetString(windowName, "DeleteShopTitle");
            var msg = _localizationService.GetString(windowName, "DeleteShopText");
            
            var userAction = await MessageBoxManager.GetMessageBoxStandard(title, msg, ButtonEnum.YesNo).ShowWindowDialogAsync(this);
            
            if (userAction != ButtonResult.Yes) return;
            _shopStorage.RemoveShop(selected);
            try
            {
                _sqliteDbContext.RemoveColumn($"{Enums.Tables.ProductShopMapping}", selected);
            }
            catch (SqliteException ex) when (ex.SqliteErrorCode == 1)
            { }
            InitializeShopSelector();
        }

        private void ShowDataFormatTooltip(object? sender, RoutedEventArgs e)
        {
            var psi = new ProcessStartInfo()
            {
                FileName = SelectGuidePage(),
                UseShellExecute = true
            };
            Process.Start(psi);
        }
        
        private static string SelectGuidePage()
        {
            var language = Thread.CurrentThread.CurrentCulture.Name;
            const string fileName = "Formats";
            var fileDirectory = Path.Combine(Environment.CurrentDirectory, "Web", "DataFormats");
            var path = Path.Combine(fileDirectory, $"{fileName}.{language}.html");
            var baseFile = Path.Combine(fileDirectory, $"{fileName}.html");

            return File.Exists(path) ? path : baseFile;
        }
    }
}