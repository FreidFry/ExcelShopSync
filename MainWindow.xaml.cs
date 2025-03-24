using System.Windows;
using ExcelShopSync.Modules;
using ExcelShopSync.Services.Base;
using ExcelShopSync.Services.Price;
using ExcelShopSync.Services.Quantity;

using static ExcelShopSync.Properties.Settings;

namespace ExcelShopSync;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        DiscountFrom.SelectedDate = DateTime.Now;
        DiscountTo.SelectedDate = DateTime.Now.AddDays(DefaultTimeOffset);
        timePicker.Value = DateTime.Today.Add(DefaultTime.ToTimeSpan());

        SetFakeDiscount_TextBox.Text = DefaultFakeDiscount.ToString();

        EditTargetList.Visibility = Visibility.Hidden;
        EditSourceList.Visibility = Visibility.Hidden;
    }

    private void GetTarget_Click(object sender, RoutedEventArgs e)
    {
        var targets = GetFile.Get<Target>(TargetFileName);

        if (targets != null)
        {
            foreach (var target in targets)
            {
                if (target != null && !FileManager.Target.Any(t => t.FileName == target.FileName))
                {
                    FileManager.Target.Add(target);
                }
            }
        }

        if (FileManager.Target.Count > 1)
        {
            EditTargetList.Visibility = Visibility.Visible;
            TargetFileName.Visibility = Visibility.Hidden;
        }
    }


    private void GetSource_Click(object sender, RoutedEventArgs e)
    {
        var sources = GetFile.Get<Source>(SourceFileName);

        if (sources != null)
        {
            foreach (var source in sources)
            {
                if (source != null && !FileManager.Source.Any(t => t.FileName == source.FileName))
                {
                    FileManager.Source.Add(source);
                }
            }
        }

        if (FileManager.Source.Count > 1)
        {
            EditSourceList.Visibility = Visibility;
            SourceFileName.Visibility = Visibility.Hidden;
        }
    }

    private void Start_Click(object sender, RoutedEventArgs e)
    {
        if (FileManager.Target.Count == 0)
        {
            MessageBox.Show("Please select target files");
            return;
        }

        if (FileManager.Source.Count == 0 && SetFakeDiscount.IsChecked != true)
        {
            MessageBox.Show("Please select source files");
            return;
        }

        if (FileManager.Source.Count > 0)
        {
            if (GetPrice.IsChecked == true) new TransferPrice().Transfer();
            if (GetQuantity.IsChecked == true) new TransferQuantity().Transfer();
            if (SynchronizeRealDiscount.IsChecked == true)
            {
                if (Discount_IgnoreManual.IsChecked == true)
                {
                    new SyncDiscount().Transfer();
                }
                else
                {
                    if (DiscountFrom.SelectedDate == null || DiscountTo.SelectedDate == null)
                    {
                        MessageBox.Show("Please select dates");
                        return;
                    }
                    new SyncDiscount().Transfer(DiscountFrom.SelectedDate, DiscountTo.SelectedDate, timePicker.Value.ToString());
                }
            }

            if (SetFakeDiscount.IsChecked == true)
            {
                if (string.IsNullOrEmpty(SetFakeDiscount_TextBox.Text) || !double.TryParse(SetFakeDiscount_TextBox.Text, out double percent))
                {
                    MessageBox.Show("Please enter a value");
                    return;
                }
                FakeDiscount.Transfer(percent);
            }

            foreach (var target in FileManager.Target)
            {
                target.ExcelPackage.Save();
            }

            MessageBox.Show("Done");
        }
    }

    private void GetPrice_Checked(object sender, RoutedEventArgs e)
    {

    }

    private void GetQuantity_Checked(object sender, RoutedEventArgs e)
    {

    }

    private void SetFakeDiscount_Checked(object sender, RoutedEventArgs e)
    {
        SetFakeDiscount_TextBox.IsEnabled = true;
        SetFakeDiscount_TextBox.Visibility = Visibility.Visible;
    }

    private void SetFakeDiscount_Unchecked(object sender, RoutedEventArgs e)
    {
        SetFakeDiscount_TextBox.IsEnabled = false;
        SetFakeDiscount_TextBox.Visibility = Visibility.Hidden;
    }

    private void SynchronizeRealDiscount_Checked(object sender, RoutedEventArgs e)
    {

    }

    private void Discount_IgnoreManual_Checked(object sender, RoutedEventArgs e)
    {
        DiscountFrom.IsEnabled = false;
        DiscountTo.IsEnabled = false;
        timePicker.IsEnabled = false;
    }

    private void Discount_IgnoreManual_Unchecked(object sender, RoutedEventArgs e)
    {
        DiscountFrom.IsEnabled = true;
        DiscountTo.IsEnabled = true;
        timePicker.IsEnabled = true;
    }

    private void EditTargetList_Click(object sender, RoutedEventArgs e)
    {
        EditFilesForChanges.OpenChanges(FileManager.Target);
    }

    private void EditSourceList_Click(object sender, RoutedEventArgs e)
    {
        EditFilesForChanges.OpenChanges( FileManager.Source);
    }
}
