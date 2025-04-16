using ExcelShopSync.Core.Models;

namespace ExcelShopSync.Infrastructure.Persistence
{
    class FileManager
    {
        public static List<Target> Target { get; set; } = [];
        public static List<Source> Source { get; set; } = [];
        

    }
}