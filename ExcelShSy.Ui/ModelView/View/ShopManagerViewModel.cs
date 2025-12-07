using Avalonia.Interactivity;
using DynamicData;
using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Core.Interfaces.DataBase;
using ExcelShSy.Core.Interfaces.Shop;
using ExcelShSy.Core.Interfaces.Storage;
using ExcelShSy.Core.Interfaces.ViewModels;
using ExcelShSy.Infrastructure.Persistence.Model;
using ExcelShSy.LocalDataBaseModule.Persistance;
using ExcelShSy.Ui.Interfaces;
using ExcelShSy.Ui.ModelView.Base;
using ExcelShSy.Ui.Windows;
using Microsoft.Data.Sqlite;
using System.Collections.ObjectModel;

namespace ExcelShSy.Ui.ModelView.View
{
    public class ShopManagerViewModel : ViewModelBase, IShopManagerViewModel
    {
        #region Dependency Injections

        private readonly IShopStorage _shopStorage;
        private readonly ILogger _logger;
        private readonly IFileProvider _fileProvider;
        private readonly ILocalizationService _localizationService;
        private readonly ISqliteDbContext _sqliteDbContext;
        private readonly IDialogService _dialogService;
        private readonly IWindowProvider _windowProvider;


        #endregion

        #region Fields

        private bool _shopChanged;
        private bool _isInitialized;
        private bool _shouldShowMessage;
        private string? _previousShopName;
        private string? _selectedShopName;

        private IShopTemplate? _currentShop;
        private readonly ObservableCollection<string?> _shopHeaders = [];
        private readonly ObservableCollection<string> _allShopList = [];

        #endregion

        #region Properties
        private string WindowName { get; }

        public ObservableCollection<string> AllShopList
        {
            get => _allShopList;
        }

        public ObservableCollection<string?> ShopHeaders
        {
            get => _shopHeaders;
        }

        public bool ShopChanged
        {
            get => _shopChanged;
            set => SetProperty(ref _shopChanged, value);
        }

        public bool IsInitialized
        {
            get => _isInitialized;
            set => SetProperty(ref _isInitialized, value);
        }

        public bool ShouldShowMessage
        {
            get => _shouldShowMessage;
            set => SetProperty(ref _shouldShowMessage, value);
        }

        public string? PreviousShop
        {
            get => _previousShopName;
            set => SetProperty(ref _previousShopName, value);
        }

        public string? SelectedShop
        {
            get => _selectedShopName;
            set
            {
                PreviousShop = _selectedShopName;
                if (SetProperty(ref _selectedShopName, value))
                    ShopChange(value);
            }
        }

        public IShopTemplate? CurrentShop
        {
            get => _currentShop;
            private set => SetProperty(ref _currentShop, value);
        }

        #endregion

        public ShopManagerViewModel(IShopStorage shopStorage, ILogger logger, IFileProvider fileProvider, ILocalizationService localizationService, ISqliteDbContext sqliteDbContext, IWindowProvider windowProvider, IDialogService dialogService)
        {
            _shopStorage = shopStorage;
            _logger = logger;
            _fileProvider = fileProvider;
            _localizationService = localizationService;
            _sqliteDbContext = sqliteDbContext;
            _dialogService = dialogService;
            _windowProvider = windowProvider;


            WindowName = GetType().Name.Replace("viewmodel", "Window", StringComparison.OrdinalIgnoreCase);

            #region Initialize Commands

            CreateShopCommand = new AsyncRelayCommands(async _ => await CreateShopAsync());
            RenameShopCommand = new AsyncRelayCommands(async _ => await RenameShopAsync());
            RemoveShopCommand = new AsyncRelayCommands(async _ => await DropShopAsync());
            SaveShopCommand = new RelayCommands(_ => SaveShop());
            LoadShopCommand = new AsyncRelayCommands(async param =>
            {
                if (param is not string shopName) return;
                await LoadShopAsync(shopName);
            });
            RemoveSelectedHeadersCommand = new AsyncRelayCommands(async param =>
            {
                if (param is not IList<object> needRemove) return;
                var needRemoveStrings = needRemove.OfType<string>().ToArray();
                await RemoveSelectedHeadersAsync(needRemoveStrings);
            });

            #endregion

            InitializeShopSelector();
        }

        private void InitializeShopSelector()
        {
            AllShopList.Clear();
            AllShopList.AddRange(_shopStorage.GetShopList());
        }

        public async Task<bool> TryCloseAsync()
        {
            if (ShopChanged)
                SaveShop();

            if (ShouldShowMessage)
            {
                var title = _localizationService.GetString(WindowName, "ConfirmSaveTitle");
                var msg = _localizationService.GetString(WindowName, "ConfirmSaveText");

                var ConfirmClose = await _dialogService.QuestionDialogAsync(title, msg);
                if (!ConfirmClose)
                    return false;
                ShouldShowMessage = false;
            }
            return true;
        }

        #region Relay Commands

        public AsyncRelayCommands CreateShopCommand { get; }
        public async Task CreateShopAsync()
        {
            string shopName = string.Empty;
            #region Initialize Message

            var buttons = new[]
            {
                _localizationService.GetString(WindowName, "CreateButton"),
                _localizationService.GetString(WindowName, "CancelButton")
            };
            var title = _localizationService.GetString(WindowName, "CreateNewShopTitle");
            var msg = _localizationService.GetString(WindowName, "CreateNewShopText");

            #endregion

            try
            {
                shopName = await _dialogService.RequiredReturnValueDialogAsync<string>(title, msg, buttons);
                if (_shopStorage.IsFileExist(shopName)) throw new IOException();


                var shop = new ShopTemplate { Name = shopName };

                _shopStorage.SaveShopTemplate(shop);
                _shopStorage.AddShop(shop.Name);
                InitializeShopSelector();
            }
            catch (OperationCanceledException)
            {
                _logger.LogInfo($"New shop creation ({shopName}) canceled by user.");
                return;
            }
            catch (IOException)
            {
                _logger.LogError($"Error creating new shop template file. This name exist ({shopName!})");
                return;
            }
        }

        public AsyncRelayCommands RemoveShopCommand { get; }
        public async Task DropShopAsync()
        {
            if (CurrentShop == null)
            {
                _logger.LogWarning("Remove shop error \"CurrentShop\" is null. ");
                return;
            }
            string shopName = CurrentShop.Name;

            #region Initialize Message

            var title = _localizationService.GetString(WindowName, "DeleteShopTitle");
            var msg = _localizationService.GetString(WindowName, "DeleteShopText");

            #endregion

            try
            {
                var confirm = await _dialogService.QuestionDialogAsync(title, msg);
                if (!confirm) return;
                _shopStorage.RemoveShop(shopName);
                _sqliteDbContext.DropColumn($"{Enums.Tables.ProductShopMapping}", shopName);
                InitializeShopSelector();
            }
            catch (OperationCanceledException)
            {
                _logger.LogInfo($"Remove shop ({shopName}) canceled by user.");
                return;
            }
            catch (SqliteException)
            {
                InitializeShopSelector();
            }
        }

        public AsyncRelayCommands RenameShopCommand { get; }
        public async Task RenameShopAsync()
        {
            #region Initialize Message

            var buttons = new[]
            {
                _localizationService.GetString(WindowName, "RenameButton"),
                _localizationService.GetString(WindowName, "CancelButton")
            };
            var title = _localizationService.GetString(WindowName, "RenameShopTitle");
            var msg = _localizationService.GetString(WindowName, "RenameShopText");
            var formattedMsg = string.Format(msg, SelectedShop);

            #endregion

            string newShopName = string.Empty;
            try
            {
                newShopName = await _dialogService.RequiredReturnValueDialogAsync<string>(title, formattedMsg, buttons);
                if (_shopStorage.IsFileExist(newShopName)) throw new IOException();

                CurrentShop.Name = newShopName;
                _shopStorage.RenameShop(CurrentShop.Name, newShopName);
                try
                {
                    _sqliteDbContext.RenameColumn($"{Enums.Tables.ProductShopMapping}", CurrentShop.Name, newShopName);
                }
                catch (SqliteException ex) when (ex.SqliteErrorCode == 1)
                {
                    _logger.LogWarning($"Error renaming column in database. This name does not exist ({newShopName})");
                    return;
                }
                InitializeShopSelector();
            }
            catch (OperationCanceledException)
            {
                _logger.LogInfo($"Rename shop ({newShopName}) canceled by user.");
                return;
            }
            catch (IOException)
            {
                _logger.LogError($"Error creating new shop template file. This name exist ({newShopName})");
                return;
            }
        }

        public RelayCommands SaveShopCommand { get; }
        public void SaveShop()
        {
            if (CurrentShop?.Name == null)
            {
                var msg = $"Shop save error \"CurrentShop\" is null ";
                _logger.LogError(msg);
                return;
            }

            CurrentShop.UnmappedHeaders = [.. ShopHeaders];
            _shopStorage.UpdateShop(CurrentShop);
            _shopStorage.SaveShopTemplate(CurrentShop);
            _sqliteDbContext.CreateColumn(CurrentShop.Name);
            _shopChanged = false;
            _shouldShowMessage = true;
        }

        public AsyncRelayCommands LoadShopCommand { get; }
        public async Task LoadShopAsync(string shopName)
        {
            var shopTemplate = _shopStorage.GetShopMapping(shopName);
            if (shopTemplate == null)
            {
                var msg = $"Shop load error. Shop template is null ({shopName})";
                _logger.LogError(msg);
                await _dialogService.ShowDefaultDialogAsync(
                    _localizationService.GetErrorString("Error"),
                    _localizationService.GetErrorString("ShopLoadErrorMessage"));
                return;
            }
            CurrentShop = shopTemplate;
            ShopHeaders.Clear();
            ShopHeaders.AddRange(CurrentShop.UnmappedHeaders);
            ShopChanged = false;
        }

        public AsyncRelayCommands RemoveSelectedHeadersCommand { get; }
        public async Task RemoveSelectedHeadersAsync(IList<string> needRemove)
        {
            ShopHeaders.Remove(needRemove);
        }

        public AsyncRelayCommands LoadHeadersFromFileCommand => new(async _ => await GetHeadersWithFileAsync());

        public async Task GetHeadersWithFileAsync()
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

            ShopHeaders.Clear();
            ShopHeaders.AddRange(values);
            _shopChanged = true;
        }

        #endregion

        public void OnChanged(object sender, RoutedEventArgs e)
        {
            if (_isInitialized)
                _shopChanged = true;
        }

        public void OnLoaded()
        {
            _isInitialized = true;
        }

        private async Task ShopChange(string shopName)
        {
            if (_shopChanged && shopName != _selectedShopName)
            {
                const string windowName = nameof(ShopManagerWindow);
                var title = _localizationService.GetString(windowName, "UnsavedChanges");
                var msg = _localizationService.GetString(windowName, "UnsavedChangesText");

                var result = await _dialogService.QuestionDialogAsync(title, msg);
                if (!result) return;
            }
            var shop = _shopStorage.GetShopMapping(shopName);
            ShopHeaders.Clear();
            ShopHeaders.Add(null);
            ShopHeaders.AddRange(shop?.UnmappedHeaders);
            CurrentShop = shop;
            _shopChanged = false;
        }


    }
}

