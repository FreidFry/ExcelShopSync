using ExcelShopSync.Modules;
using ExcelShopSync.Services.Base;
using System.Windows;
using System.Windows.Controls;

namespace ExcelShopSync
{
    public partial class EditList : Window
    {
        public EditList()
        {
            InitializeComponent();
        }

        public void EditList_Closed(object sender, EventArgs e)
        {
            EditFilesForChanges.Close();
        }

        public void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var item = button.DataContext;

            if (item != null)
                EditFilesForChanges.Remove(item);
        }

        public void ShowInfo_Click(object sender, RoutedEventArgs e)
        {
            if(sender is Button button && button.DataContext is Target target)
            {
                target.ShowInfo();
            }
            else if (sender is Button btn && btn.DataContext is Source source)
            {
                source.ShowInfo();
            }
        }
    }
}