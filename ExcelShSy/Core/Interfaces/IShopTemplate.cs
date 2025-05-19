using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelShSy.Core.Interfaces
{
    public interface IShopTemplate
    {
        IReadOnlyList<string> columns { get; }
        IReadOnlyDictionary<string, string> Availability { get; }
    }
}
