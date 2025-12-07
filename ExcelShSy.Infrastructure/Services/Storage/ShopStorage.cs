using ExcelShSy.Core.Enums;
using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Core.Interfaces.Shop;
using ExcelShSy.Core.Interfaces.Storage;
using ExcelShSy.Infrastructure.Persistence.Model;
using MsBox.Avalonia;
using MsBox.Avalonia.Base;
using MsBox.Avalonia.Enums;
using Newtonsoft.Json;

namespace ExcelShSy.Infrastructure.Services.Storage
{
    /// <summary>
    /// Provides file-system backed storage for shop templates and raises notifications when shops change.
    /// </summary>
    public class ShopStorage : IShopStorage
    {
        /// <summary>
        /// Stores the directory path where shop template JSON files are persisted.
        /// </summary>
        private readonly string _directoryPath = Path.Combine(Environment.CurrentDirectory, "UserData", "Shops");

        private readonly IShopTemplateFactory _shopFactory;
        private readonly IColumnMappingStorage _columnMappingStorage;
        private readonly ILocalizationService _localizationService;
        private readonly IMessagesService<IMsBox<ButtonResult>> _messages;

        /// <summary>
        /// Gets or sets the list of loaded shop templates.
        /// </summary>
        public List<IShopTemplate> Shops { get; set; }
        
        public ShopStorage(IShopTemplateFactory shopFactory, IColumnMappingStorage columnMapping, ILocalizationService localizationService, IMessagesService<IMsBox<ButtonResult>> messages)
        {
            _shopFactory = shopFactory;
            _columnMappingStorage = columnMapping;
            _localizationService = localizationService;
            _messages = messages;
            Shops = Initializer();
        }

        /// <summary>
        /// Loads existing shop templates from disk and configures column mappings.
        /// </summary>
        /// <returns>The list of loaded shop templates.</returns>
        private List<IShopTemplate> Initializer()
        {
            var result = new List<IShopTemplate>();

            CheckExistDirectory();

            var fullShopPath = Directory.GetFiles(_directoryPath, "*.json").ToList();

            var serializer = CreateJsonSerializer();

            foreach (var shopPath in fullShopPath)
            {
                var shop = FetchShopTemplate(shopPath, serializer);
                if (shop == null) continue;
                result.Add(shop);
                _columnMappingStorage.AddColumn(shop);
            }

            if (result.Count < 1)
            {
                var title = _localizationService.GetErrorString("NoFetchShops");
                var msg = _localizationService.GetErrorString("NoFetchShops");
                _messages.GetMessageBoxStandard(title, msg)
                    .ShowAsync();
            }
            return result;
        }

        /// <summary>
        /// Adds a shop template by reading the corresponding JSON file based on its name.
        /// </summary>
        /// <param name="shopName">The shop name identifying the JSON file.</param>
        public void AddShop(string shopName)
        {
            var path = Path.Combine(_directoryPath, $"{shopName}.json");
            if (!File.Exists(path)) return;
            var serializer = CreateJsonSerializer();
            var shop = FetchShopTemplate(path, serializer);
            if(shop == null) return;
            AddShop(shop);
            _columnMappingStorage.AddColumn(shop);
        }

        /// <summary>
        /// Renames an existing shop template file and updates the stored template.
        /// </summary>
        /// <param name="oldName">The current shop name.</param>
        /// <param name="newName">The new shop name.</param>
        public void RenameShop(string oldName, string newName)
        {
            var path = Path.Combine(_directoryPath, $"{oldName}.json");
            if (!File.Exists(path))
            {
                var title = _localizationService.GetErrorString("NoShopFile");
                var msg = _localizationService.GetErrorString("NoShopFileText");
                _messages.GetMessageBoxStandard(title, msg, MyButtonEnum.Ok, MyIcon.Error).ShowAsync();
                return;
            }

            var renamedPath = Path.Combine(_directoryPath, $"{newName}.json");
            if (IsFileExist(renamedPath)) return;
            File.Move(path, renamedPath);
            var serializer = CreateJsonSerializer();
            var shop = FetchShopTemplate(renamedPath, serializer);
            if (shop == null) return;
            shop.Name = newName;
            SaveShopTemplate(shop);
            ReplaceShopWithName(oldName, shop);
        }

        /// <summary>
        /// Determines whether a shop template file exists for the specified path or name.
        /// </summary>
        /// <param name="path">The absolute path or shop name.</param>
        /// <returns><c>true</c> if the file exists or the name conflicts; otherwise, <c>false</c>.</returns>
        public bool IsFileExist(string path)
        {
            if (!path.EndsWith(".json"))
                path = Path.Combine(_directoryPath, $"{path}.json");
            if (File.Exists(path))
            {
                var title = _localizationService.GetErrorString("ShopExist");
                var msg = _localizationService.GetErrorString("ShopExistText");
                _messages.GetMessageBoxStandard(title, msg, MyButtonEnum.Ok, MyIcon.Error).ShowAsync();
                return true;
            } 
            return false;
        }
        
        /// <summary>
        /// Replaces an existing shop in the collection by name.
        /// </summary>
        /// <param name="previousShop">The name of the shop to replace.</param>
        /// <param name="newShop">The replacement shop template.</param>
        /// <returns><c>true</c> if the shop was replaced; otherwise, <c>false</c>.</returns>
        private bool ReplaceShopWithName(string previousShop, IShopTemplate newShop)
        {
            var index = Shops.FindIndex(s => s.Name == previousShop);
            if (index == -1) return false;
            Shops[index] = newShop.Clone();
            return true;
        }
        
        /// <summary>
        /// Retrieves a copy of the shop template for the specified shop name.
        /// </summary>
        /// <param name="shopName">The name of the shop to retrieve.</param>
        /// <returns>The cloned shop template, or <c>null</c> if not found.</returns>
        public IShopTemplate? GetShopMapping(string shopName) => Shops.FirstOrDefault(s => s.Name == shopName)?.Clone();

        /// <summary>
        /// Reads and deserializes a shop template from the provided file path.
        /// </summary>
        /// <param name="shopPath">The path to the shop JSON file.</param>
        /// <param name="serializer">The JSON serializer to use.</param>
        /// <returns>The deserialized shop template, or <c>null</c> if deserialization fails.</returns>
        private IShopTemplate? FetchShopTemplate(string shopPath, JsonSerializer serializer)
        {
            using var stream = new FileStream(shopPath, FileMode.Open);
            using var reader = new StreamReader(stream);
            using var jsonReader = new JsonTextReader(reader); 

            var shop = serializer.Deserialize<ShopTemplate>(jsonReader) ?? _shopFactory.Create();

            if (string.IsNullOrEmpty(shop.Name))
            {
                shop.Name = Path.GetFileNameWithoutExtension(shopPath);
            }
            return shop;
        }

        /// <summary>
        /// Updates the stored shop template, prompting the user to add it if it does not yet exist.
        /// </summary>
        /// <param name="updatedShop">The updated shop template.</param>
        public async void UpdateShop(IShopTemplate updatedShop)
        {
            if (ReplaceShopWithName(updatedShop.Name, updatedShop)) return;
            var title = _localizationService.GetErrorString("ShopNotFound");
            var msg = _localizationService.GetErrorString("ShopNotFoundText");
            var formatedText = string.Format(msg, updatedShop.Name);
            
            var msBox = _messages.GetMessageBoxStandard(title, formatedText, MyButtonEnum.YesNo);
            var result = await msBox.ShowAsync();
            if (result == ButtonResult.Yes)
                Shops.Add(updatedShop);
        }
        
        /// <summary>
        /// Persists the specified shop template to disk.
        /// </summary>
        /// <param name="shop">The shop template to save.</param>
        public void SaveShopTemplate(IShopTemplate? shop)
        {
            if (shop == null) return;
            CheckExistDirectory();

            var fullShopPath = Path.Combine(_directoryPath, $"{shop.Name}.json");
            using var stream = new FileStream(fullShopPath, FileMode.Create);
            using var writer = new StreamWriter(stream);
            using var jsonWriter = new JsonTextWriter(writer);
            CreateJsonSerializer().Serialize(jsonWriter, shop);
        }

        /// <summary>
        /// Creates a JSON serializer configured for persisting shop templates.
        /// </summary>
        /// <returns>A configured JSON serializer instance.</returns>
        private static JsonSerializer CreateJsonSerializer() => new()
        {
            TypeNameHandling = TypeNameHandling.Objects
            
        };

        /// <summary>
        /// Ensures that the storage directory exists, creating it if necessary.
        /// </summary>
        private void CheckExistDirectory()
        {
            if (Directory.Exists(_directoryPath)) return;
            Directory.CreateDirectory(_directoryPath);
        }

        /// <summary>
        /// Gets the list of shop names currently loaded.
        /// </summary>
        /// <returns>A list of shop names.</returns>
        public List<string> GetShopList() => Shops.Select(x => x.Name).ToList();
        
        public event Action<string>? ShopsChanged;

        /// <summary>
        /// Adds the provided shop template and raises the <see cref="ShopsChanged"/> event.
        /// </summary>
        /// <param name="shop">The shop template to add.</param>
        public void AddShop(IShopTemplate shop)
        {
            Shops.Add(shop);
            ShopsChanged?.Invoke(shop.Name);
        }
        
        /// <summary>
        /// Removes the specified shop template and deletes the associated file.
        /// </summary>
        /// <param name="shopName">The name of the shop to remove.</param>
        public void RemoveShop(string shopName)
        {
            var shop = Shops.First(s => s.Name == shopName);
            Shops.Remove(shop);
            var path = Path.Combine(_directoryPath, $"{shopName}.json");
            File.Delete(path);
            ShopsChanged?.Invoke(shopName);
        }
    }
}