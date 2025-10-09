using Avalonia;
using OfficeOpenXml;

namespace ExcelShSy.Ui;

public static class Program
{
        [STAThread]
        public static void Main(string[] args)
        {
            ExcelPackage.License.SetNonCommercialPersonal("Freid4");
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }
    
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .WithInterFont()
                .LogToTrace();
}