using Avalonia.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace FurudType.App.ViewModels;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private string? _textToDisplay = string.Empty;

    [RelayCommand]
    private void HandleTextInputCommand(TextInputEventArgs e)
    {
        TextToDisplay += e.Text;
    }
}
