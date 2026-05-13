using CommunityToolkit.Mvvm.ComponentModel;

namespace FurudType.App.ViewModels;

public partial class CharacterViewModel : ViewModelBase
{
    [ObservableProperty]
    private char _character;

    [ObservableProperty]
    private bool _isCurrent;
}
