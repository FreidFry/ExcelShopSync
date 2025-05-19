using ExcelShSy.Core.Interfaces;
using static ExcelShSy.Infrastracture.Persistance.ShopData.AvailabilityMapping;

namespace ExcelShSy.Infrastracture.Persistance.ShopData.Datas
{
    public record RozetkaData : IShopTemplate
    {
        public IReadOnlyList<string> columns => new List<string>
        {
            "ID",
            "OFFERID",
            "CID",
            "Категорія",
            "Артикул",
            "Назва",
            "Назва (укр)",
            "Серія",
            "Ціна",
            "Стара ціна",
            "Ціна промо",
            "Залишки",
            "Мінімальна кількість при замовленні",
            "Максимальна кількість при замовленні",
            "Виробник",
            "Наявність",
            "Статус",
            "Повний опис (UA/RU)",
            "Повний опис (UA)",
            "Зображення",
            "Гарантія|20769",
            "Довжина|24935",
            "Дополнительный текст;RU|21875",
            "Дополнительный текст;UA|21875",
            "Доставка/Оплата;RU|2019",
            "Доставка/Оплата;UA|2019",
            "Кнопка передзамовлення|232597",
            "Потужність на квадратний метр|24931",
            "Призначення|243713",
            "Загальна потужність|26288",
            "Площа|35484",
            "Призначення|176472",
            "Термін доставки|252319",
            "Країна-виробник товару|98900",
            "Монтаж|220893",
            "Ширина|24948"
        };

        public IReadOnlyDictionary<string, string> Availability => new Dictionary<string, string>
        {
            { InStock, "В наявності" },
            { OutOfStock, "Не в наявності" },
            { OnOrder, "Под заказ" },
            { ReadyToGo, "Готов к отправке" }
        };
    }
}
