using CommunityToolkit.Mvvm.ComponentModel;

namespace FurudType.App.ViewModels.Keyboard;

public partial class KeyItemViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _displayText = string.Empty;

    [ObservableProperty]
    private bool _isCurrent = false;
}
