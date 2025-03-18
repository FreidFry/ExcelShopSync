using System.IO;
using Microsoft.Win32;
using System.Windows.Controls;
using ExcelShopSync.Modules;
namespace ExcelShopSync.Services.Base
{
    class GetFile
    {
        public static T? Get<T>(Label label) where T : FileBase
        {
            OpenFileDialog fileDialog = new()
            {
                Filter = "Excel Files|*.xls;*.xlsx;*.xlsm"
            };

            if (fileDialog.ShowDialog() == false)
            {
                return null;
            }

            label.Content = Path.GetFileName(fileDialog.FileName);
            return (T?)Activator.CreateInstance(typeof(T), fileDialog.FileName);
        }
    }
}
