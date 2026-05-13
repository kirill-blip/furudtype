using System;

using Avalonia.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace FurudType.App.ViewModels;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private string? _targetText = "Hello World";

    [ObservableProperty]
    private int _errorsCount;

    [ObservableProperty]
    private int _correctCount;

    private int _currentIndex;

    [RelayCommand]
    private void HandleTextInput(TextInputEventArgs e)
    {
        if (string.IsNullOrEmpty(e.Text) || string.IsNullOrEmpty(_targetText))
        {
            return;
        }

        if (_currentIndex >= _targetText.Length)
        {
            return;
        }

        foreach (char inputChar in e.Text)
        {
            if (_currentIndex >= _targetText.Length)
            {
                break;
            }

            if (_targetText[_currentIndex] != inputChar)
            {
                ErrorsCount++;
            }
            else
            {
                CorrectCount++;
            }

            _currentIndex++;
        }
    }
}
