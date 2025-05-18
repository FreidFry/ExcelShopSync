using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelShSy.Infrastracture.Persistance.Model
{
    public record ShopInfoModel(
        IReadOnlyList<string> Columns,
        IReadOnlyDictionary<string, string> AvailabilityMapping
    );
}
