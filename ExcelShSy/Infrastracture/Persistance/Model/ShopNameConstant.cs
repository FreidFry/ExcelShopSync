namespace ExcelShSy.Infrastracture.Persistance.Model
{
    public static class ShopNameConstant
    {
        public const string Horoshop = "Horoshop";
        public const string Prom = "Prom";
        public const string Rozetka = "Rozetka";
        public const string Epicenter = "Epicenter";
        public const string Ibud = "Ibud";
        public const string Unknown = "Unknown";

        public static readonly IReadOnlyList<string> All = new[]
        {
            Rozetka, Epicenter, Prom, Horoshop, Ibud, Unknown
        };
    }
}