﻿namespace ExcelShopSync.Core.Static
{
    public static class Shops
    {
        public static List<string> AllShops = new()
        {
            Rozetka,
            Epicenter,
            Prom,
            Horoshop,
            Ibud,
            Unknown
        };
        public const string Horoshop = "Horoshop";
        public const string Prom = "Prom";
        public const string Rozetka = "Rozetka";
        public const string Epicenter = "Epicenter";
        public const string Ibud = "Ibud";

        public const string Unknown = "Unknown";
    }
}
