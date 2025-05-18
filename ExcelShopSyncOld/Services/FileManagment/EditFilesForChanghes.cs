using ExcelShopSync.Core.Models;
using ExcelShopSync.Infrastructure.Persistence;

namespace ExcelShopSync.Services.FileManagment
{
    public static class EditFilesForChanges
    {
        private static EditList? window;
        private static object? Filelist;
        public static void OpenChanges<T>(List<T> list)
        {
            if (window == null || !window.IsVisible)
            {
                window = new EditList();
                window.Show();
                window.FileListBox.ItemsSource = list;
                Filelist = list;
            }
        }

        public static void Close()
        {
            if (window != null && window.IsVisible)
            {
                window.Close();
                window = null;
            }
        }

        public static void Remove(object s)
        {
            if (Filelist is List<Source>) FileManager.Source.Remove((Source)s);
            if (Filelist is List<Target>) FileManager.Target.Remove((Target)s);
            window?.FileListBox.Items.Refresh();
        }
    }

}
