using Avalonia;
using OfficeOpenXml;

namespace ExcelShSy.Ui;

public class Program
{
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args)
        {
            ExcelPackage.License.SetNonCommercialPersonal("Freid4");
            BuildAvaloniaApp()
                .StartWithClassicDesktopLifetime(args);
        }

        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .WithInterFont()
                .UseSkia()
                .LogToTrace()
            ;
}