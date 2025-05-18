using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelShSy.Features.Interfaces
{
    public interface IExcelHelper
    {
        List<string> GetUndefinedHeaders();
        Dictionary<string, int> GetHeaders();
        string GetLang();
    }
}
