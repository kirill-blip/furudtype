using Avalonia.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace FurudType.App.ViewModels;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private string? _textToDisplay;

    [RelayCommand]
    private void HandleTextInput(TextInputEventArgs e)
    {
        TextToDisplay += e.Text;
    }
}
