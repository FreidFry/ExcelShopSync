using ExcelShSy.Core.Interfaces.Shop;

using static ExcelShSy.Infrastructure.Persistance.DefaultValues.AvailabilityConstant;


namespace ExcelShSy.Infrastructure.Persistance.ShopData.Datas
{
    public record HoroshopData : IShopTemplate
    {
        public IReadOnlyList<string> UnmappedHeaders =>
        [
            "Артикул",
            "Родительский артикул",
            "Артикул для отображения на сайте",
            "Название модификации (UA)",
            "Название модификации (RU)",
            "Название (UA)",
            "Название (RU)",
            "Бренд",
            "Раздел",
            "Цена",
            "Старая цена",
            "Валюта",
            "Отображать",
            "Наличие",
            "Дополнительные разделы",
            "Фото",
            "Галерея",
            "Обзор 360",
            "Алиас",
            "Ссылка",
            "Дата добавления",
            "HTML title (UA)",
            "HTML title (RU)",
            "Единицы измерения",
            "META keywords (UA)",
            "META keywords (RU)",
            "META description (UA)",
            "META description (RU)",
            "h1 заголовок (UA)",
            "h1 заголовок (RU)",
            "Минимальный заказ",
            "Поставщик",
            "Иконки",
            "Популярность",
            "Скидка %",
            "Описание товара (UA)",
            "Описание товара (RU)",
            "Количество",
            "Короткое описание (UA)",
            "Короткое описание (RU)",
            "Тип гарантии",
            "Цвет",
            "Гарантийный срок, мес.",
            "Дата и время окончания акции",
            "Текст акции (UA)",
            "Текст акции (RU)",
            "Описание для маркетплейсов (UA)",
            "Описание для маркетплейсов (RU)",
            "Выгружать на маркетплейсы",
            "Состояние товара",
            "Только для взрослых",
            "Код УКТ ВЭД",
            "«Оплата частями» ПриватБанка",
            "«Покупка частями» от monobank",
            "Уникальный код налога",
            "Штрихкод",
            "Код производителя товара (MPN)",
            "На складе для Prom",
            "Электронный товар",
        ];

        public IReadOnlyDictionary<string, string> AvailabilityMap => new Dictionary<string, string>
        {
            { InStock, "В наявності" },
            { OnOrder, "Під замовлення" },
            { OutOfStock, "Не в наявності" },
            { ReadyToGo, "в наявності" }
        };

        public string DataFormat => "dd-MM-yyyy HH:mm:ss";
    }
}
