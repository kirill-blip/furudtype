using System;
using System.Collections.ObjectModel;

using Avalonia.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using FurudType.Core.Models;

namespace FurudType.App.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    private ObservableCollection<CharacterViewModel> _characters = [];

    [ObservableProperty]
    private Lesson _lesson;

    [ObservableProperty]
    private int _errorsCount;

    [ObservableProperty]
    private int _correctCount;

    [ObservableProperty]
    private bool _isExerciseFinished = false;

    private Exercise _currentExercise;

    private int _currentIndex;

    public MainViewModel()
    {
        Exercise firstExercise = new Exercise()
        {
            Title = "First",
            Text = "ffjj jfjf fjjf",
        };

        Exercise secondExercise = new Exercise()
        {
            Title = "Second",
            Text = "ddkk dkdk dkkd",
        };

        Lesson = new Lesson()
        {
            Title = "Middle line",
            Exercises = { firstExercise, secondExercise },
        };

        _currentExercise = Lesson.Exercises[0];

        foreach (char character in Lesson.Exercises[0].Text)
        {
            CharacterViewModel characterViewModel = new() { Character = character, IsCurrent = false };

            Characters.Add(characterViewModel);
        }

        Characters[0].IsCurrent = true;
    }

    [RelayCommand]
    private void HandleTextInput(TextInputEventArgs e)
    {
        if (string.IsNullOrEmpty(e.Text))
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

        if (_currentIndex == Characters.Count)
        {
            IsExerciseFinished = true;
        }
    }

    [RelayCommand]
    private void ChangeLesson(Exercise exercise)
    {
        Characters.Clear();
        _currentExercise = exercise;

        IsExerciseFinished = false;
        _currentIndex = 0;
        CorrectCount = 0;
        ErrorsCount = 0;

        foreach (char character in exercise.Text)
        {
            CharacterViewModel characterViewModel = new() { Character = character, IsCurrent = false };

            Characters.Add(characterViewModel);
        }

        Characters[0].IsCurrent = true;
    }

    [RelayCommand]
    private void ChangeToNextExercise()
    {
        int index = Lesson.Exercises.IndexOf(_currentExercise);
        if (index == Lesson.Exercises.Count - 1)
        {
            return;
        }

        Characters.Clear();

        IsExerciseFinished = false;
        _currentExercise = Lesson.Exercises[index + 1];
        _currentIndex = 0;
        CorrectCount = 0;
        ErrorsCount = 0;

        foreach (char character in _currentExercise.Text)
        {
            CharacterViewModel characterViewModel = new()
            {
                Character = character,
                IsCurrent = false
            };

            Characters.Add(characterViewModel);
        }

        Characters[0].IsCurrent = true;
    }
}
