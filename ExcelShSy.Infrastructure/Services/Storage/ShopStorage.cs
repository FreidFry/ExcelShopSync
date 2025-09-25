using System.ComponentModel;
using ExcelShSy.Core.Interfaces.Shop;
using ExcelShSy.Core.Interfaces.Storage;
using ExcelShSy.Infrastructure.Persistance.Model;
using Newtonsoft.Json;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace ExcelShSy.Infrastructure.Services.Storage
{
    public class ShopStorage : IShopStorage, INotifyPropertyChanged
    {
        private readonly string _directoryPath = Path.Combine(Environment.CurrentDirectory, "UserData", "Shops");

        private readonly IShopTemplateFactory _shopFactory;
        private readonly IColumnMappingStorage _columnMappingStorage;
        public List<IShopTemplate> Shops { get; set; }
        
        public ShopStorage(IShopTemplateFactory shopFactory, IColumnMappingStorage columnMapping)
        {
            _shopFactory = shopFactory;
            _columnMappingStorage = columnMapping;
            Shops = Initializer();
        }

        private List<IShopTemplate> Initializer()
        {
            var result = new List<IShopTemplate>();

            CheckExistPath();

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
                MessageBoxManager.GetMessageBoxStandard("No fetch shops!","No fetch shops!").ShowAsync();
            
            return result;
        }

        public void AddShop(string shopName)
        {
            var path = Path.Combine(_directoryPath, $"{shopName}.json");
            if (!File.Exists(path)) return;
            var serializer = CreateJsonSerializer();
            var shop = FetchShopTemplate(path, serializer);
            if(shop == null) return;
            Shops.Add(shop);
            _columnMappingStorage.AddColumn(shop);
        }
        public IShopTemplate? GetShopMapping(string shopName) => Shops.FirstOrDefault(s => s.Name == shopName)?.Clone();

        private IShopTemplate? FetchShopTemplate(string shopPath, JsonSerializer serializer)
        {
            using var stream = new FileStream(shopPath, FileMode.Open);
            using var reader = new StreamReader(stream);
            using var jsonReader = new JsonTextReader(reader);

            IShopTemplate? shop = serializer.Deserialize<ShopTemplate>(jsonReader);
            
            if (shop == null) shop = _shopFactory.Create();
            if (string.IsNullOrEmpty(shop.Name))
            {
                shop.Name = Path.GetFileNameWithoutExtension(shopPath);
            }
            return shop;
        }

        public async void UpdateShop(IShopTemplate updatedShop)
        {
            var index = Shops.FindIndex(s => s.Name == updatedShop.Name);
            if (index != -1)
                Shops[index] = updatedShop;
            else
            {
                var msBox = MessageBoxManager.GetMessageBoxStandard("Shop Not Found",$"Shop {updatedShop.Name} not found. Do you want to add it?", ButtonEnum.YesNo);
                var result = await msBox.ShowAsync();
                if (result == ButtonResult.Yes)
                    Shops.Add(updatedShop);
            }
        }
        
        public void SaveShopTemplate(IShopTemplate? shop)
        {
            if (shop == null) return;
            CheckExistPath();

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

        private void CheckExistPath()
        {
            if (Directory.Exists(_directoryPath)) return;
            Directory.CreateDirectory(_directoryPath);
        }

        public List<string> GetShopList() => Shops.Select(x => x.Name).ToList();
        
        public event PropertyChangedEventHandler? PropertyChanged;
        
    }
}