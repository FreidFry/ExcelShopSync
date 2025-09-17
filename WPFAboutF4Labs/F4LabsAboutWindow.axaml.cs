using System.Diagnostics;
using System.Reflection;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ExcelShSy.Localization;


namespace WPFAboutF4Labs
{
    public partial class F4LabsAboutWindow : Window
    {
        public F4LabsAboutWindow()
        {
            Init();
        }
        private void Init()
        {
            InitializeComponent();

            Version.Text = GetAssemblyVersion();
            Copyrignt.Text = SetCopyright();
        }

        private static string GetAssemblyVersion()
        {
            var text = GetLocalizationInCode.GetLocalizate("F4LabsAboutWindow","Version");
            return$"{text} {Assembly.GetEntryAssembly()?.GetName().Name?.Split(".")[0]} {Assembly.GetEntryAssembly()?.GetName().Version}\n";
        }

        private static string SetCopyright()
        {
            var CurrentYear = DateTime.Now.Year.ToString();
            var metadataAttributes = Assembly.GetEntryAssembly()?.GetCustomAttributes<AssemblyMetadataAttribute>();

            string? releaseYear = metadataAttributes?
                .FirstOrDefault(attr => attr.Key == "ReleaseYear")
                ?.Value;

            var text = GetLocalizationInCode.GetLocalizate("F4LabsAboutWindow", "Copyright");

            if (!string.IsNullOrEmpty(releaseYear) && releaseYear == CurrentYear)
                return $"© {CurrentYear} {text}";

            return $"© {releaseYear}-{CurrentYear} {text}";
        }
        private void PayPalDonateButton_Click(object sender, RoutedEventArgs e)
        {
            OpenBrowser("https://www.paypal.com/donate/?hosted_button_id=PMZZY5MTVUH8Y");
        }

        private void BoostyDonateButton_Click(object sender, RoutedEventArgs e)
        {
            OpenBrowser("https://boosty.to/freid4/donate");
        }

        private void MonoBankJarButton_Click(object sender, RoutedEventArgs e)
        {
            OpenBrowser("https://send.monobank.ua/jar/53EokQuT4A");
        }

        private static void OpenBrowser(string url)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
