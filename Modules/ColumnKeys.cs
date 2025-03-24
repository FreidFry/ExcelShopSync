namespace ExcelShopSync.Modules
{
    public static class ColumnKeys
    {
        public const string Article = "Article";
        public const string CompectArticle = "CompectArticle";
        public const string Price = "Price";
        public const string CompectPrice = "CompectPrice";
        public const string PriceOld = "PriceOld";
        public const string Quantity = "Quantity";
        public const string Availability = "Availability";
        public const string Discount = "Discount";
        public const string DiscountFrom = "DiscountFrom";
        public const string DiscountTo = "DiscountTo";
    }

    public static class AvailabilityKeys
    {
        public const string InStock = "В наличии";
        public const string OutOfStock = "Нет в наличии";
        public const string OnOrder = "Под заказ";
        public const string ReadyToGo = "Готов к отправке";
    }
}