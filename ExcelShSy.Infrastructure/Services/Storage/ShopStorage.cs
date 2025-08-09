using ExcelShSy.Core.Interfaces.Shop;
using Newtonsoft.Json;
using System.IO;
using System.Windows;

namespace ExcelShSy.Infrastructure.Services.Storage
{
    public class ShopStorage : IShopStorage
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

            List<string> FullShopPath = Directory.GetFiles(_directoryPath, "*.json").ToList();

            var serializer = CreateJsonSerializer();

            foreach (var ShopPath in FullShopPath)
            {
                var shop = FetchShopTemplate(ShopPath, serializer);
                if (shop != null)
                {
                    result.Add(shop);
                    _columnMappingStorage.AddColumn(shop);
                }
            }
            if (result.Count < 1)
                MessageBox.Show("No fetch shops!");
            return result;
        }

        public IShopTemplate? GetShopMapping(string shopName) => Shops.FirstOrDefault(s => s.Name == shopName)?.Clone();

        private IShopTemplate? FetchShopTemplate(string shopPath, JsonSerializer serializer)
        {
            using var stream = new FileStream(shopPath, FileMode.Open);
            using var reader = new StreamReader(stream);
            using var jsonReader = new JsonTextReader(reader);

            var shop = serializer.Deserialize<IShopTemplate>(jsonReader);
            if (shop == null) shop = _shopFactory.Create();
            if (string.IsNullOrEmpty(shop.Name))
            {
                shop.Name = Path.GetFileNameWithoutExtension(shopPath);
            }
            return shop;
        }

        public void UpdateShop(IShopTemplate updatedShop)
        {
            var index = Shops.FindIndex(s => s.Name == updatedShop.Name);
            if (index != -1)
                Shops[index] = updatedShop;
            else
            {
                var question = MessageBox.Show($"Shop {updatedShop.Name} not found. Do you want to add it?", "Shop Not Found", MessageBoxButton.YesNo);
                if (question == MessageBoxResult.Yes)
                    Shops.Add(updatedShop);
            }
        }

        public void SaveShopTemplate(IShopTemplate shop)
        {
            if (shop == null) return;
            CheckExistPath();

            var FullShopPath = Path.Combine(_directoryPath, $"{shop.Name}.json");
            using var stream = new FileStream(FullShopPath, FileMode.Create);
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
            if (!Directory.Exists(_directoryPath))
            {
                Directory.CreateDirectory(_directoryPath);
            }
        }

        public List<string> GetShopList() => Shops.Select(x => x.Name).ToList();
    }
}