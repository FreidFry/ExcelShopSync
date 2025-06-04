using ExcelShSy.Core.Interfaces.Shop;

using static ExcelShSy.Infrastructure.Persistance.DefaultValues.AvailabilityConstant;

namespace ExcelShSy.Infrastructure.Persistance.ShopData.Datas
{
    public record EpicenterData : IShopTemplate
    {
        public IReadOnlyList<string> Columns =>
        [
            "№",
            "Артикул*",
            "Ціна*",
            "Стара ціна",
            "Фото товару*",
            "Наявність*",
            "Основна категорія*",
            "Бере участь в акціях",
            "Додаткові характеристики (text)[ru]",
            "Додаткові характеристики (text)[ua]",
            "Вид* (multiselect)",
            "Колір* (multiselect)",
            "Мінімальна кратність товару* (float)",
            "Опис (text)[ru]",
            "Опис (text)[ua]",
            "Колекція (select)",
            "Вага* (float) (кг)",
            "Серія (select)",
            "Модель виробника (референцiя) (string)",
            "Штрих код (array)",
            "Одиниця виміру та кількість* (select)",
            "Висота упаковки* (float) (мм)",
            "Глибина упаковки* (float) (мм)",
            "Вага упаковки* (float) (г)",
            "Ширина упаковки* (float) (мм)",
            "Модель (multiselect)",
            "Бренд* (select)",
            "Країна-виробник* (select)",
            "Набір атрибутів*",
            "id (не редагується)",
            "Статус (не редагується)"
        ];

        public IReadOnlyDictionary<string, string> Availability => new Dictionary<string, string>
        {
            { InStock, "в наявності" },
            { OutOfStock, "немає в наявності" },
            { OnOrder, "в наявності" },
            { ReadyToGo, "в наявності" },
        };

        public string DataFormat => "";
    }
}
