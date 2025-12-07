using System.Net;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ExcelShSy.Core.Enums;
using ExcelShSy.Core.Interfaces.Common;
using MsBox.Avalonia;
using MsBox.Avalonia.Base;
using MsBox.Avalonia.Enums;

namespace ExcelShSy.Ui.Windows;

public partial class CheckConnectionWindow : Window
{
    private const string ClassName = "CheckConnectionWindow";
    
    private readonly CancellationTokenSource _cancellationTokenSource = new();

    private readonly ILocalizationService _localizationService;
    private readonly ILogger _logger;
    private readonly IMessagesService<IMsBox<ButtonResult>> _messages;
#if DESIGNER

    public CheckConnectionWindow()
    {
        InitializeComponent();
    }
#endif
    
    public CheckConnectionWindow(ILocalizationService localizationService, ILogger logger, IMessagesService<IMsBox<ButtonResult>> messages)
    {
        InitializeComponent();
        
        _localizationService = localizationService;
        _logger = logger;
        _messages = messages;
    }

    public async Task<bool> CheckGitHubConnection(TextBlock? text = null, ProgressBar? progress = null)
    {
        if (text != null)
            text.Text = _localizationService.GetString(ClassName, "CheckConnection");
        if (progress != null)
            progress.IsIndeterminate = true;
        try
        {
             await Dns.GetHostEntryAsync("github.com", _cancellationTokenSource.Token);
             
            if (text != null)
                text.Text = _localizationService.GetString(ClassName, "Connected");
            return true;
        }
        catch (OperationCanceledException e)
        {
            _logger.LogInfo(e.Message);
            return false;
        }
        catch (Exception e)
        {
            if (text != null)
                text.Text = _localizationService.GetString(ClassName, "ConnectionError");
            if (progress != null)
            {
                progress.IsIndeterminate = false;
                progress.Value = 1;
            }
            var title = _localizationService.GetErrorString("NetworkErrorTitle");
            var msg = _localizationService.GetErrorString("NetworkErrorText");
            await _messages.GetMessageBoxStandard(title, msg, MyButtonEnum.Ok, MyIcon.Error)
                .ShowWindowAsync();
            _logger.LogError(e.Message);
            return false;
        }
        finally
        {
            Close();
        }
    }

    private void CancelButton_OnClick(object? sender, RoutedEventArgs e)
    {
        _cancellationTokenSource.Cancel();
        CancelToken?.Invoke(this, EventArgs.Empty);
    }
    
    public event EventHandler? CancelToken;
}