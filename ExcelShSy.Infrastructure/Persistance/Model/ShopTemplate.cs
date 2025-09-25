using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ExcelShSy.Core.Interfaces.Shop;

namespace ExcelShSy.Infrastructure.Persistance.Model
{
    public class ShopTemplate : IShopTemplate
    {
        private string _name;
        private List<string>? _unmappedHeaders;
        private IReadOnlyDictionary<string, string>? _availabilityMap;
        private string? _dataFormat;
        private string? _article;
        private string? _price;
        private string? _oldPrice;
        private string? _availability;
        private string? _quantity;
        private string? _discount;
        private string? _discountDateStart;
        private string? _discountDateEnd;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public List<string> UnmappedHeaders { get => _unmappedHeaders ??= [];
            set
            {
                _unmappedHeaders = value;
                OnPropertyChanged();
            } }

        public IReadOnlyDictionary<string, string> AvailabilityMap
        {
            get => _availabilityMap ??= new Dictionary<string, string>(4);
            set
            {
                _availabilityMap = value;
                OnPropertyChanged();
            }
        }

        public string? DataFormat { get => _dataFormat;
            set
            {
                if (value != null) _dataFormat = value;
                OnPropertyChanged();
            }
        }

        public string? Article
        {
            get => _article;
            set
            {
                _article = value;
                OnPropertyChanged();
            }
        }

        public string? Price
        {
            get => _price;
            set
            {
                _price = value;
                OnPropertyChanged();
            }
        }

        public string? OldPrice
        {
            get => _oldPrice;
            set
            {
                _oldPrice = value;
                OnPropertyChanged();
            }
        }

        public string? Availability
        {
            get => _availability;
            set
            {
                _availability = value;
                OnPropertyChanged();
            }
        }

        public string? Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                OnPropertyChanged();
            }
        }

        public string? Discount
        {
            get => _discount;
            set
            {
                _discount = value;
                OnPropertyChanged();
            }
        }

        public string? DiscountDateStart
        {
            get => _discountDateStart;
            set
            {
                _discountDateStart = value;
                OnPropertyChanged();
            }
        }

        public string? DiscountDateEnd
        {
            get => _discountDateEnd;
            set
            {
                _discountDateEnd = value;
                OnPropertyChanged();
            }
        }

        public IShopTemplate Clone()
        {
            return new ShopTemplate
            {
                Name = this.Name,
                UnmappedHeaders = this.UnmappedHeaders,
                AvailabilityMap = this.AvailabilityMap,
                DataFormat = this.DataFormat,
                Article = this.Article,
                Price = this.Price,
                OldPrice = this.OldPrice,
                Availability = this.Availability,
                Quantity = this.Quantity,
                Discount = this.Discount,
                DiscountDateStart = this.DiscountDateStart,
                DiscountDateEnd = this.DiscountDateEnd
            };
        }
        
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}
