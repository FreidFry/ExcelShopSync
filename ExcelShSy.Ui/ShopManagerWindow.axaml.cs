using System.Collections.ObjectModel;
using ExcelShSy.Core.Interfaces.Shop;
using ExcelShSy.Core.Interfaces.Common;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using DynamicData;
using ExcelShSy.Core.Interfaces.DataBase;
using ExcelShSy.Core.Interfaces.Storage;
using ExcelShSy.Infrastructure.Persistence.Model;
using ExcelShSy.LocalDataBaseModule.Persistance;
using Microsoft.Data.Sqlite;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia.Models;
using MsBox.Avalonia.ViewModels.Commands;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

namespace ExcelShSy.Ui
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
        private int _currentSelection;

        #endregion
        
        #region Fields
        
        public ObservableCollection<string> LoadedShops { get; set; } = [];
        
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
        
        private readonly ObservableCollection<string> _magazineSelectorItems = [];
        
        #endregion

        public new event PropertyChangedEventHandler? PropertyChanged;
        
        public ShopManagerWindow()
        {
            InitializeComponent();
        }
        
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
                await SaveShop();
                Close();
            }
        }

        private void InitializeShopSelector()
        {
            LoadedShops.Clear();
            LoadedShops.AddRange(_shopStorage.GetShopList());
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
                    Header = _localizationService.GetString(nameof(ShopManagerWindow), "Rename"),
                    Command = new RelayCommand(_ =>
                    {
                        if(list.SelectedItem is string selected) RenameShopCommand(selected);
                    })
                },
                new()
                {
                    Header = _localizationService.GetString(nameof(ShopManagerWindow), "DeleteButton"),
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
                        await SaveShop();
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
                CurrentShopHeaders!.Clear();
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
            _magazineSelectorItems.Add(shopName);
            MagazineSelector.SelectedItem = _magazineSelectorItems.Count - 1;
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

        private async void SaveShopTemplate_Click(object sender, RoutedEventArgs e)
        {
            await SaveShop();
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
                shopName = msBox.InputValue.Replace(" ", "_").ToUpper();
                if (userAction == cancelButton || userAction == null) break;
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

        private async Task SaveShop()
        {
            if (!_shopChanged)
            {
                var windowName = nameof(ShopManagerWindow);
                var title = _localizationService.GetString(windowName, "ConfirmSaveTitle");
                var msg = _localizationService.GetString(windowName, "ConfirmSaveText");
                
                var msBox = MessageBoxManager.GetMessageBoxStandard( title, msg, ButtonEnum.YesNo);
                var result = await msBox.ShowWindowDialogAsync(this);
                if (result != ButtonResult.Yes)
                    return;
            }

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
            CurrentShop.UnmappedHeaders = list;
            _shopStorage.UpdateShop(CurrentShop);
            _shopStorage.SaveShopTemplate(CurrentShop);
            _shopChanged = false;
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
                //                             VVVVVVVVVVVVVVVVVVVVVVVVVVV
                renamedShop = msBox.InputValue.Replace(" ", "_").ToUpper();
                if (userAction == cancelButton || userAction == null!) break;
            }
            while (string.IsNullOrWhiteSpace(renamedShop));
            
            if (userAction != renameButton) return;
            _currentShop.Name = renamedShop;
            _shopStorage.RenameShop(oldName!, renamedShop);
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
            var windowName = nameof(ShopManagerWindow);
            var title = _localizationService.GetString(windowName, "DeleteShopTitle");
            var msg = _localizationService.GetString(windowName, "DeleteShopText");
            
            var userAction = await MessageBoxManager.GetMessageBoxStandard(title, msg, ButtonEnum.YesNo).ShowWindowDialogAsync(this);
            
            if (userAction != ButtonResult.Yes || userAction == null!) return;
            _shopStorage.RemoveShop(selected);
            try
            {
                _sqliteDbContext.RemoveColumn($"{Enums.Tables.ProductShopMapping}", selected);
            }
            catch (SqliteException ex) when (ex.SqliteErrorCode == 1)
            { }
            InitializeShopSelector();
        }
    }
}