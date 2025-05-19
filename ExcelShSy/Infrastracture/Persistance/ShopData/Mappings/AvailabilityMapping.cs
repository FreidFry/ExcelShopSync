using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelShSy.Infrastracture.Persistance.ShopData
{
    public static class AvailabilityMapping
    {
        public const string InStock = "В наличии";
        public const string OutOfStock = "Нет в наличии";
        public const string OnOrder = "Под заказ";
        public const string ReadyToGo = "Готов к отправке";
    }
}
