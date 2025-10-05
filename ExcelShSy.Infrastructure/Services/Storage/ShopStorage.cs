using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Core.Interfaces.Shop;
using ExcelShSy.Core.Interfaces.Storage;
using ExcelShSy.Infrastructure.Persistence.Model;
using Newtonsoft.Json;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace ExcelShSy.Infrastructure.Services.Storage
{
    public class ShopStorage : IShopStorage
    {
        private readonly string _directoryPath = Path.Combine(Environment.CurrentDirectory, "UserData", "Shops");

        private readonly IShopTemplateFactory _shopFactory;
        private readonly IColumnMappingStorage _columnMappingStorage;
        private readonly ILocalizationService _localizationService;
        public List<IShopTemplate> Shops { get; set; }
        
        public ShopStorage(IShopTemplateFactory shopFactory, IColumnMappingStorage columnMapping, ILocalizationService localizationService)
        {
            _shopFactory = shopFactory;
            _columnMappingStorage = columnMapping;
            _localizationService = localizationService;
            Shops = Initializer();
        }

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
                MessageBoxManager.GetMessageBoxStandard(title, msg)
                    .ShowAsync();
            }
            return result;
        }

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

        public void RenameShop(string oldName, string newName)
        {
            var path = Path.Combine(_directoryPath, $"{oldName}.json");
            if (!File.Exists(path))
            {
                var title = _localizationService.GetErrorString("NoShopFile");
                var msg = _localizationService.GetErrorString("NoShopFileText");
                MessageBoxManager.GetMessageBoxStandard(title, msg, ButtonEnum.Ok, Icon.Error).ShowAsync();
                return;
            }

            var renamedPath = Path.Combine(_directoryPath, $"{newName}.json");
            if (IsFileNotExist(renamedPath)) return;
            File.Move(path, renamedPath);
            var serializer = CreateJsonSerializer();
            var shop = FetchShopTemplate(renamedPath, serializer);
            if (shop == null) return;
            shop.Name = newName;
            SaveShopTemplate(shop);
            ReplaceShopWithName(oldName, shop);
        }

        public bool IsFileNotExist(string path)
        {
            if (!path.EndsWith(".json"))
                path = Path.Combine(_directoryPath, $"{path}.json");
            if (File.Exists(path))
            {
                var title = _localizationService.GetErrorString("ShopExist");
                var msg = _localizationService.GetErrorString("ShopExistText");
                MessageBoxManager.GetMessageBoxStandard(title, msg, ButtonEnum.Ok, Icon.Error).ShowAsync();
                return true;
            } 
            return false;
        }
        
        private bool ReplaceShopWithName(string previousShop, IShopTemplate newShop)
        {
            var index = Shops.FindIndex(s => s.Name == previousShop);
            if (index == -1) return false;
            Shops[index] = newShop.Clone();
            return true;
        }
        
        public IShopTemplate? GetShopMapping(string shopName) => Shops.FirstOrDefault(s => s.Name == shopName)?.Clone();

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

        public async void UpdateShop(IShopTemplate updatedShop)
        {
            if (ReplaceShopWithName(updatedShop.Name, updatedShop)) return;
            var title = _localizationService.GetErrorString("ShopNotFound");
            var msg = _localizationService.GetErrorString("ShopNotFoundText");
            var formatedText = string.Format(msg, updatedShop.Name);
            
            var msBox = MessageBoxManager.GetMessageBoxStandard(title, formatedText, ButtonEnum.YesNo);
            var result = await msBox.ShowAsync();
            if (result == ButtonResult.Yes)
                Shops.Add(updatedShop);
        }
        
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

        private static JsonSerializer CreateJsonSerializer() => new()
        {
            TypeNameHandling = TypeNameHandling.Objects
            
        };

        private void CheckExistDirectory()
        {
            if (Directory.Exists(_directoryPath)) return;
            Directory.CreateDirectory(_directoryPath);
        }

        public List<string> GetShopList() => Shops.Select(x => x.Name).ToList();
        
        public event Action<string>? ShopsChanged;

        public void AddShop(IShopTemplate shop)
        {
            Shops.Add(shop);
            ShopsChanged?.Invoke(shop.Name);
        }
        
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