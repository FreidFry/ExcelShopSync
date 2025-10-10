using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.Json;
using ExcelShSy.Core.Interfaces.Common;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace ExcelShSy.Ui.Utils;

public class UpdateManager(ILocalizationService localizationService, IAppSettings appSettings)
{
    public async Task<bool> Check()
    {
        try
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.UserAgent.ParseAdd("request");
            client.Timeout = new TimeSpan(0, 0, 10);
            var json = await client.GetStringAsync("https://api.github.com/repos/FreidFry/ExcelShopSync/releases/latest");
            using var doc = JsonDocument.Parse(json);
            var gitVerse = doc.RootElement.GetProperty("tag_name").GetString()?.Replace("v", "");
            var currentVersion = Assembly.GetExecutingAssembly().GetName().Version;
            if (!Version.TryParse(gitVerse, out var latestVersion))
                throw new Exception("Failed to parse version from GitHub.");

            return currentVersion < latestVersion;
        }
        catch (Exception e) when (e is HttpRequestException or TaskCanceledException)
        {
            var title = localizationService.GetErrorString("NetworkErrorTitle");
            var msg = localizationService.GetErrorString("NetworkErrorText");
            await MessageBoxManager.GetMessageBoxStandard(title, msg, ButtonEnum.Ok, Icon.Error)
                .ShowWindowAsync();
            return false;
        }
        catch (Exception e)
        {
            var title = localizationService.GetErrorString("UnknownErrorTitle");
            var msg = localizationService.GetErrorString("UnknownErrorText") + $"\n\n{e.Message}";
            await MessageBoxManager.GetMessageBoxStandard(title, msg, ButtonEnum.Ok, Icon.Error)
                .ShowWindowAsync();
            return false;
        }
    }

    private async Task<(bool isSuccess, string? filePath)> Download()
    {
        var assetName = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? "ExShSy-win-x64.zip"
            : "ExShSy-linux-x64.tar.gz";
        var savePath = Path.Combine(Path.GetTempPath(), assetName);

        var client = new HttpClient();
        client.DefaultRequestHeaders.UserAgent.ParseAdd("request");

        var json = await client.GetStringAsync("https://api.github.com/repos/FreidFry/ExcelShopSync/releases/latest");
        using var doc = JsonDocument.Parse(json);

        string? downloadUrl = null;
        foreach (var asset in doc.RootElement.GetProperty("assets").EnumerateArray())
        {
            if (asset.GetProperty("name").GetString() != assetName) continue;

            downloadUrl = asset.GetProperty("browser_download_url").GetString();
            break;
        }

        if (downloadUrl == null)
        {
            var title = localizationService.GetErrorString("UnsupportedOSUpdateTitle");
            var msg = localizationService.GetErrorString("UnsupportedOSUpdateText");
            await MessageBoxManager
                .GetMessageBoxStandard(title, msg, ButtonEnum.Ok, Icon.Error)
                .ShowWindowAsync();
            return (false, null);
        }

        var data = await client.GetByteArrayAsync(downloadUrl);
        await File.WriteAllBytesAsync(savePath, data);
        return (true, savePath);
    }

    public async void UpdateApp()
    {
        try
        {
            var (success, updateFile) = await Download();
            if (!success)
            {
                var title = localizationService.GetErrorString("UnsupportedOSUpdateTitle");
                var msg = localizationService.GetErrorString("UnsupportedOSUpdateText");
                await MessageBoxManager
                    .GetMessageBoxStandard(title, msg, ButtonEnum.Ok, Icon.Error)
                    .ShowWindowAsync();
                return;
            }

            var fi = new FileInfo(updateFile!);
            while (!fi.Exists || fi.Length == 0)
                await Task.Delay(50);

            appSettings.LastUpdateCheck = DateTime.Now.Date;
            appSettings.SaveSettings(appSettings);
            var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

            var updaterPath = isWindows
                ? Path.Combine(Environment.CurrentDirectory, "ExcelShSy.Setup.exe")
                : Path.Combine(Environment.CurrentDirectory, "ExcelShSy.Setup");
            var finalExecutable = isWindows ? "ExcelShSy.Ui.exe" : "ExcelShSy.Ui";

            var psi = new ProcessStartInfo
            {
                FileName = updaterPath,
                Arguments = $"\"{updateFile}\" \"{Environment.CurrentDirectory}\" \"{finalExecutable}\"",
                UseShellExecute = true
            };

            Process.Start(psi);
            Environment.Exit(0);
        }
        catch (Exception ex) when (ex is HttpRequestException)
        {
            var title = localizationService.GetErrorString("NetworkErrorTitle");
            var msg = localizationService.GetErrorString("NetworkErrorText");
            await MessageBoxManager.GetMessageBoxStandard(title, msg, ButtonEnum.Ok, Icon.Error)
                .ShowWindowAsync();
        }
        catch (Exception ex)
        {
            var title = localizationService.GetErrorString("UnknownErrorTitle");
            var msg = localizationService.GetErrorString("UnknownErrorText") + $"\n\n{ex.Message}";
            await MessageBoxManager.GetMessageBoxStandard(title, msg, ButtonEnum.Ok, Icon.Error)
                .ShowWindowAsync();
        }
    }
}