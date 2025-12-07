using System.ComponentModel;
using System.Runtime.CompilerServices;
using ExcelShSy.Core.Interfaces.Shop;
using static ExcelShSy.Infrastructure.Persistence.DefaultValues.AvailabilityConstant;

namespace ExcelShSy.Infrastructure.Persistence.Model
{
    /// <summary>
    /// Concrete implementation of <see cref="IShopTemplate"/> that raises change notifications for UI bindings.
    /// </summary>
    public class ShopTemplate : IShopTemplate
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        private string _name;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        private HashSet<string> _unmappedHeaders = [];
        private Dictionary<string, string?>? _availabilityMap;
        private string? _dataFormat;
        private string? _article;
        private string? _price;
        private string? _oldPrice;
        private string? _availability;
        private string? _quantity;
        private string? _discount;
        private string? _discountDateStart;
        private string? _discountDateEnd;

        /// <inheritdoc />
        public string Name
        {
            get => _name;
            set
            {
                _name = value.Replace(" ", "_").ToUpper();
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public HashSet<string?> UnmappedHeaders
        {
            get => _unmappedHeaders!;
            set
            {
                if (value is null)
                    value = [];
                else
                {
                    value.RemoveWhere(v => v is null);
                    _unmappedHeaders = value!;
                }

                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public Dictionary<string, string?> AvailabilityMap
        {
            get => _availabilityMap ??= new Dictionary<string, string?>
            {
                {InStock, null},
                {OutOfStock, null},
                {OnOrder, null},
                {ReadyToGo, null}
            };
            set
            {
                _availabilityMap = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public string? DataFormat
        {
            get => _dataFormat;
            set
            {
                if (value != null) _dataFormat = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public string? Article
        {
            get => _article;
            set
            {
                _article = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public string? Price
        {
            get => _price;
            set
            {
                _price = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public string? OldPrice
        {
            get => _oldPrice;
            set
            {
                _oldPrice = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public string? Availability
        {
            get => _availability;
            set
            {
                _availability = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public string? Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public string? Discount
        {
            get => _discount;
            set
            {
                _discount = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public string? DiscountDateStart
        {
            get => _discountDateStart;
            set
            {
                _discountDateStart = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public string? DiscountDateEnd
        {
            get => _discountDateEnd;
            set
            {
                _discountDateEnd = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public IShopTemplate Clone()
        {
            return new ShopTemplate
            {
                Name = this.Name,
                UnmappedHeaders = [.. this.UnmappedHeaders],
                AvailabilityMap = new Dictionary<string, string?>(this.AvailabilityMap),
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

        /// <summary>
        /// Notifies subscribers that a property value has changed.
        /// </summary>
        /// <param name="propertyName">The name of the changed property.</param>
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}
