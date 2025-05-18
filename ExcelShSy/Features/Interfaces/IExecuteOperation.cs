using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelShSy.Features.Interfaces
{
    public interface IExecuteOperation
    {
        string Name { get; }
        void Execute();
    }
}
