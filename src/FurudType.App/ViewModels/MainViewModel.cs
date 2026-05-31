using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Avalonia.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using FurudType.Core.Models;
using FurudType.Core.Repositories;

namespace FurudType.App.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    private ObservableCollection<CharacterViewModel> _characters = [];

    public KeyboardViewModel KeyboardViewModel { get; private set; }

    [ObservableProperty]
    private Lesson _lesson;

    [ObservableProperty]
    private int _errorsCount;

    [ObservableProperty]
    private int _correctCount;

    [ObservableProperty]
    private bool _isExerciseFinished = false;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasExercises))]
    private bool _isLessonFinished = false;

    public bool HasExercises => !IsLessonFinished;

    private readonly List<Lesson> _lessons = [];

    private Exercise _currentExercise;

    private int _currentIndex;

    private readonly ILessonRepository _lessonRepository;

    public MainViewModel(ILessonRepository lessonRepository, KeyboardViewModel keyboardViewModel)
    {
        _lessonRepository = lessonRepository;
        _lessons = _lessonRepository.GetAll();
        Lesson = _lessons[0];

        _currentExercise = Lesson.Exercises[0];

        foreach (char character in Lesson.Exercises[0].Text)
        {
            CharacterViewModel characterViewModel = new() { Character = character, IsCurrent = false };

            Characters.Add(characterViewModel);
        }

        Characters[0].IsCurrent = true;

        KeyboardViewModel = keyboardViewModel;
        KeyboardViewModel.ChangeKeyItem(Characters[0].Character);
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

            if (inputChar != Characters[_currentIndex].Character)
            {
                ErrorsCount++;
                continue;
            }

            if (inputChar == ' ' && Characters[_currentIndex].Character != ' ')
            {
                return;
            }

            Characters[_currentIndex].IsCorrect = true;
            Characters[_currentIndex].IsCurrent = false;

            CorrectCount++;
            _currentIndex++;

            if (_currentIndex >= Characters.Count)
            {
                Characters[^1].IsCurrent = true;
                KeyboardViewModel.ChangeKeyItem(Characters[^1].Character);
            }
            else
            {
                Characters[_currentIndex].IsCurrent = true;
                KeyboardViewModel.ChangeKeyItem(Characters[_currentIndex].Character);
            }
        }

        if (_currentIndex == Characters.Count)
        {
            IsExerciseFinished = true;

            int index = Lesson.Exercises.IndexOf(_currentExercise);
            if (index == Lesson.Exercises.Count - 1)
            {
                IsLessonFinished = true;
            }
        }
    }

    [RelayCommand]
    private void ChangeExercise(Exercise exercise)
    {
        Characters.Clear();
        _currentExercise = exercise;

        IsExerciseFinished = false;
        IsLessonFinished = false;
        _currentIndex = 0;
        CorrectCount = 0;
        ErrorsCount = 0;

        foreach (char character in exercise.Text)
        {
            CharacterViewModel characterViewModel = new()
            {
                Character = character,
                IsCurrent = false
            };

            Characters.Add(characterViewModel);
        }

        Characters[0].IsCurrent = true;
        KeyboardViewModel.ChangeKeyItem(Characters[0].Character);
    }

    [RelayCommand]
    private void ChangeToNextExercise()
    {
        int index = Lesson.Exercises.IndexOf(_currentExercise);
        if (index == Lesson.Exercises.Count - 1)
        {
            IsLessonFinished = true;
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
        KeyboardViewModel.ChangeKeyItem(Characters[0].Character);
    }

    [RelayCommand]
    private void ChangeLessonToNext()
    {
        int index = _lessons.IndexOf(Lesson);
        if (index == _lessons.Count - 1)
        {
            return;
        }

        _currentIndex = 0;
        CorrectCount = 0;
        ErrorsCount = 0;

        Characters.Clear();
        IsExerciseFinished = false;
        IsLessonFinished = false;

        Lesson = _lessons[index + 1];
        _currentExercise = Lesson.Exercises[0];
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
        KeyboardViewModel.ChangeKeyItem(Characters[0].Character);
    }
}
