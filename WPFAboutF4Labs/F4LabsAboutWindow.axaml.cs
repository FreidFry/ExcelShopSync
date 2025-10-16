using System.Diagnostics;
using System.Reflection;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ExcelShSy.Core.Interfaces.Common;

namespace WPFAboutF4Labs
{
    public partial class F4LabsAboutWindow : Window
    {
        private readonly ILocalizationService _localizationService;
        private readonly ILogger _logger;
        
#if DESIGNER
        public F4LabsAboutWindow()
        { }        
#endif
        
        public F4LabsAboutWindow(ILocalizationService localizationService, ILogger logger)
        {
            _localizationService = localizationService;
            _logger = logger;
            
            Init();
        }
        private void Init()
        {
            InitializeComponent();

            Version.Text = GetAssemblyVersion();
            Copyrignt.Text = SetCopyright();
        }

        private string GetAssemblyVersion()
        {
            var text = _localizationService.GetString("F4LabsAboutWindow","Version");
            var version = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly()!.Location).ProductVersion!
                .Split("+")[0];
            return$"{text} {Assembly.GetEntryAssembly()?.GetName().Name?.Split(".")[0]} {version}\n";
        }

        private string SetCopyright()
        {
            var currentYear = DateTime.Now.Year.ToString();
            var metadataAttributes = Assembly.GetEntryAssembly()?.GetCustomAttributes<AssemblyMetadataAttribute>();

            var releaseYear = metadataAttributes?
                .FirstOrDefault(attr => attr.Key == "ReleaseYear")
                ?.Value;

            var text = _localizationService.GetString("F4LabsAboutWindow", "Copyright");

            if (!string.IsNullOrEmpty(releaseYear) && releaseYear == currentYear)
                return $"© {currentYear} {text}";

            return $"© {releaseYear}-{currentYear} {text}";
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
