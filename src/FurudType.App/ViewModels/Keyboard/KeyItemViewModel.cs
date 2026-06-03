using System;
using System.Threading;
using System.Threading.Tasks;

using Avalonia.Media;

using CommunityToolkit.Mvvm.ComponentModel;

namespace FurudType.App.ViewModels.Keyboard;

public partial class KeyItemViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _displayText = string.Empty;

    [ObservableProperty]
    private bool _isCurrent = false;

    [ObservableProperty]
    private bool _isIncorrectKey = false;

    [ObservableProperty]
    private bool _isHome = false;

    [ObservableProperty]
    private IBrush _backgroundBrush = Brushes.AliceBlue;

    private CancellationTokenSource? _resetCts;

    partial void OnIsCurrentChanged(bool value)
    {
        UpdateBackground();
    }

    partial void OnIsIncorrectKeyChanged(bool value)
    {
        UpdateBackground();

        if (value)
        {
            _resetCts?.Cancel();
            _resetCts = new CancellationTokenSource();

            _ = ResetIncorrectAfterDelayAsync(TimeSpan.FromMilliseconds(300), _resetCts.Token);
        }
        else
        {
            _resetCts?.Cancel();
            _resetCts = null;
        }
    }

    private void UpdateBackground()
    {
        if (IsIncorrectKey)
        {
            BackgroundBrush = Brushes.Red;
            return;
        }

        if (IsCurrent)
        {
            BackgroundBrush = Brushes.LimeGreen;
            return;
        }

        BackgroundBrush = Brushes.AliceBlue;
    }

    private async Task ResetIncorrectAfterDelayAsync(TimeSpan delay, CancellationToken token)
    {
        try
        {
            await Task.Delay(delay, token);
            if (!token.IsCancellationRequested && IsIncorrectKey)
            {
                IsIncorrectKey = false;
            }
        }
        catch (OperationCanceledException)
        {
        }
    }
}
