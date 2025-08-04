using System.IO;
using System.Windows;


namespace ExcelShSy.Guide
{
    public partial class WebView : Window
    {
        public WebView()
        {
            InitializeComponent();
            InitWebViewAsync();
        }

        private static string SelectGuidePage()
        {
            var language = Thread.CurrentThread.CurrentCulture.Name;
            var fileName = "Guid";
            var fileDirectory = Path.Combine(Environment.CurrentDirectory, "Web");
            var path = Path.Combine(fileDirectory, $"{fileName}.{language}.html");
            var baseFile = Path.Combine(fileDirectory, $"{fileName}.html");

            if (File.Exists(path))
                return path;
            return baseFile;
        }

        private async void InitWebViewAsync()
        {
            await Web.EnsureCoreWebView2Async();

            string filePath = SelectGuidePage();
            Uri uri = new(filePath);
            Web.Source = uri;
        }
    }
}
