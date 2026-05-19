using System.Collections.ObjectModel;

using Avalonia.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace FurudType.App.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    private ObservableCollection<CharacterViewModel> _characters = [];

    [ObservableProperty]
    private string? _targetText = "Hello World";

    [ObservableProperty]
    private int _errorsCount;

    [ObservableProperty]
    private int _correctCount;

    private int _currentIndex;

    public MainViewModel()
    {
        foreach (char character in _targetText)
        {
            CharacterViewModel characterViewModel = new() { Character = character, IsCurrent = false };

            Characters.Add(characterViewModel);
        }

        Characters[0].IsCurrent = true;
    }

    [RelayCommand]
    private void HandleTextInput(TextInputEventArgs e)
    {
        if (string.IsNullOrEmpty(e.Text) || string.IsNullOrEmpty(_targetText))
        {
            return;
        }

        if (_currentIndex >= Characters.Count)
        {
            return;
        }

        foreach (char inputChar in e.Text)
        {
            if (_currentIndex >= Characters.Count)
            {
                break;
            }

            if (inputChar == ' ' && Characters[_currentIndex].Character != ' ')
            {
                continue;
            }

            if (inputChar == ' ' && Characters[_currentIndex].Character == ' ')
            {
                Characters[_currentIndex].IsCurrent = false;
                Characters[_currentIndex].IsCorrect = true;

                _currentIndex++;

                if (_currentIndex >= Characters.Count)
                {
                    Characters[^1].IsCurrent = true;
                }
                else
                {
                    Characters[_currentIndex].IsCurrent = true;
                }

                continue;
            }

            if (Characters[_currentIndex].Character != inputChar)
            {
                ErrorsCount++;
                Characters[_currentIndex].IsCorrect = false;
            }
            else
            {
                CorrectCount++;

                Characters[_currentIndex].IsCorrect = true;
            }

            Characters[_currentIndex].IsCurrent = false;
            _currentIndex++;

            if (_currentIndex >= Characters.Count)
            {
                Characters[^1].IsCurrent = true;
            }
            else
            {
                Characters[_currentIndex].IsCurrent = true;
            }
        }
    }
}
