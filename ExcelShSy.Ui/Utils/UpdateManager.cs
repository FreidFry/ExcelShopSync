using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.Json;
using ExcelShSy.Core.Interfaces.Common;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace ExcelShSy.Ui.Utils;

public class UpdateManager(ILocalizationService localizationService, IAppSettings appSettings, ILogger logger)
{
    private string? GitVersion;

    public async Task<bool> CheckUpdateAsync(bool withErrorsMessage)
    {
        try
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.UserAgent.ParseAdd("request");
            client.Timeout = TimeSpan.FromSeconds(10);
            var json = await client.GetStringAsync("https://api.github.com/repos/FreidFry/ExcelShopSync/releases/latest");
            using var doc = JsonDocument.Parse(json);
            GitVersion = doc.RootElement.GetProperty("tag_name").GetString()?.Replace("v", "");
#if  DEBUG
            return true;
#endif
            return !string.IsNullOrEmpty(GitVersion);
        }
        catch (Exception e) when (e is HttpRequestException or TaskCanceledException)
        {
            logger.LogError(e.Message);
            if (withErrorsMessage)
            {
                var title = localizationService.GetErrorString("NetworkErrorTitle");
                var msg = localizationService.GetErrorString("NetworkErrorText");
                await MessageBoxManager.GetMessageBoxStandard(title, msg, ButtonEnum.Ok, Icon.Error)
                    .ShowWindowAsync();
            }
            return false;
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            if (withErrorsMessage)
            {
                var title = localizationService.GetErrorString("UnknownErrorTitle");
                var msg = localizationService.GetErrorString("UnknownErrorText") + $"\n\n{e.Message}";
                await MessageBoxManager.GetMessageBoxStandard(title, msg, ButtonEnum.Ok, Icon.Error)
                    .ShowWindowAsync();
            }
            return false;
        }
        
    }
    
    public bool CheckVersion()
    {
        try
        {
            var currentVersion = Version.Parse(Assembly.GetEntryAssembly()!.GetCustomAttribute<AssemblyInformationalVersionAttribute>()!.InformationalVersion
                .Split("+")[0]);
            if (!Version.TryParse(GitVersion, out var latestVersion))
                throw new Exception("Failed to parse version from GitHub.");
            #if DEBUG
            return true;
#endif
            return currentVersion < latestVersion;
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            return false;
        }
    }

    private async Task<(bool isSuccess, string? filePath)> DownloadAsync()
    {
        try
        {
            var assetName = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? $"ExShSy-{GitVersion}-win-x64.zip"
                : $"ExShSy-{GitVersion}-linux-x64.tar.gz";
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

            using var response = await client.GetAsync(downloadUrl, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();
        
            await using var stream = await response.Content.ReadAsStreamAsync();
            await using var file = File.Create(savePath);
            await stream.CopyToAsync(file);
            return (true, savePath);
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            return (false, null);
        }
        
    }

    public async Task UpdateAppAsync()
    {
        try
        {
            var (success, updateArchive) = await DownloadAsync();
            if (!success)
            {
                var title = localizationService.GetErrorString("UnsupportedOSUpdateTitle");
                var msg = localizationService.GetErrorString("UnsupportedOSUpdateText");
                await MessageBoxManager
                    .GetMessageBoxStandard(title, msg, ButtonEnum.Ok, Icon.Error)
                    .ShowWindowAsync();
                return;
            }

            var fi = new FileInfo(updateArchive!);

            appSettings.LastUpdateCheck = DateTime.Now.Date;
            appSettings.SaveSettings(appSettings);
            var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            var curDir = Environment.CurrentDirectory;

            var updaterAsm = Assembly.GetAssembly(typeof(Setup.Updater));
            var updName = updaterAsm!.GetName().Name;
            var updPath = Path.Combine(curDir, $"{updName}{SetupWindowsOrLinuxExtension(isWindows)}");

            var tempUpdater = Path.Combine(
                curDir,
                Path.GetFileNameWithoutExtension(updPath) + "_tmp" + Path.GetExtension(updPath));

            File.Copy(updPath, tempUpdater, true);

            var finalFileName = Path.Combine(curDir, $"{Assembly.GetEntryAssembly().GetName().Name}{SetupWindowsOrLinuxExtension(isWindows)}");

            var updateDir = Path.Combine(curDir);

            var psi = new ProcessStartInfo
            {
                FileName = tempUpdater,
                Arguments = $"\"{updateArchive}\" \"{updateDir}\" \"{finalFileName}\"",
                UseShellExecute = false
            };

            Process.Start(psi);
            Environment.Exit(0);
        }
        catch (HttpRequestException e)
        {
            logger.LogError(e.Message);
            await ShowErrorAsync("NetworkErrorTitle", "NetworkErrorText");
            
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            await ShowErrorAsync("UnknownErrorTitle", "UnknownErrorText", e.Message);
        }
    }

    private static string SetupWindowsOrLinuxExtension(bool check)
    {
        return check ? ".exe" : "";
    }

    private async Task ShowErrorAsync(string titleKey, string msgKey, string? extra = null)
    {
        var title = localizationService.GetErrorString(titleKey);
        var msg = localizationService.GetErrorString(msgKey);
        if (extra != null) msg += $"\n\n{extra}";

        await MessageBoxManager
            .GetMessageBoxStandard(title, msg, ButtonEnum.Ok, Icon.Error)
            .ShowWindowAsync();
    }
}