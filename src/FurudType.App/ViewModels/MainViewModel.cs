using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Timers;

using Avalonia.Input;
using Avalonia.Threading;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using FurudType.Core;
using FurudType.Core.Models;
using FurudType.Core.Repositories;

namespace FurudType.App.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private const double CpmDisplayThresholdSeconds = 0;
    private const double IdleThresholdSeconds = 0.5;
    private const double DecayRatePerSecond = 30.0;
    private const int TimerIntervalMs = 250;

    private double _cpmSmoothed = 0.0;
    private const double CpmSmoothingAlpha = 0.25;
    private const double MinElapsedSecondsForDisplay = 0.2;
    private const double MinElapsedSecondsForStable = 0.6;
    private const int MaxCpmInitial = 0;

    [ObservableProperty]
    private ObservableCollection<CharacterViewModel> _characters = [];

    public KeyboardViewModel KeyboardViewModel { get; private set; }

    [ObservableProperty]
    private Lesson _lesson;

    [ObservableProperty]
    private bool _isExerciseFinished = false;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasExercises))]
    private bool _isLessonFinished = false;

    [ObservableProperty]
    private int _accurancy = 100;

    [ObservableProperty]
    private int _errors = 0;

    [ObservableProperty]
    private int _cpm = 0;

    [ObservableProperty]
    private int _finalCpm = 0;

    public bool HasExercises => !IsLessonFinished;

    private readonly List<Lesson> _lessons = [];

    private Exercise _currentExercise;

    private int _currentIndex;

    private int _totalPressedKeys;

    private readonly MetricsCalculator _metricsCalculator;

    private readonly ILessonRepository _lessonRepository;

    private DateTime? _exerciseStartTime;

    private DateTime? _lastInputTime;
    private DateTime _lastDecayUpdate;
    private readonly Timer _idleTimer;

    public MainViewModel(ILessonRepository lessonRepository,
                         KeyboardViewModel keyboardViewModel,
                         MetricsCalculator metricsCalculator)
    {
        _lessonRepository = lessonRepository;
        _metricsCalculator = metricsCalculator;
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

        _idleTimer = new Timer(TimerIntervalMs);
        _idleTimer.Elapsed += OnIdleTimerElapsed;
        _idleTimer.AutoReset = true;
        _idleTimer.Start();
    }

    private void OnIdleTimerElapsed(object? sender, ElapsedEventArgs e)
    {
        if (!_lastInputTime.HasValue)
        {
            return;
        }

        var now = DateTime.UtcNow;
        var idle = (now - _lastInputTime.Value).TotalSeconds;
        if (idle < IdleThresholdSeconds)
        {
            return;
        }

        var delta = (now - _lastDecayUpdate).TotalSeconds;
        if (delta <= 0)
        {
            _lastDecayUpdate = now;
            return;
        }

        var decrement = (int)Math.Round(DecayRatePerSecond * delta);
        if (decrement <= 0)
        {
            _lastDecayUpdate = now;
            return;
        }

        Dispatcher.UIThread.Post(() =>
        {
            _cpmSmoothed = Math.Max(0.0, _cpmSmoothed - decrement);
            Cpm = (int)Math.Round(_cpmSmoothed);
        });

        _lastDecayUpdate = now;
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

        _lastInputTime = DateTime.UtcNow;
        _lastDecayUpdate = _lastInputTime.Value;

        _totalPressedKeys++;
        if (_totalPressedKeys == 1)
        {
            _exerciseStartTime = DateTime.UtcNow;
        }

        foreach (char inputChar in e.Text)
        {
            if (_currentIndex >= Characters.Count)
            {
                break;
            }

            if (inputChar != Characters[_currentIndex].Character)
            {
                Errors++;
                KeyboardViewModel.VizualizeIncorrectKey(inputChar);
                continue;
            }

            Characters[_currentIndex].IsCorrect = true;
            Characters[_currentIndex].IsCurrent = false;

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

        Accurancy = (int)_metricsCalculator.CalculateAccurancy(_currentIndex, _totalPressedKeys);

        if (_exerciseStartTime.HasValue)
        {
            TimeSpan elapsed = DateTime.UtcNow - _exerciseStartTime.Value;
            int calculated = _metricsCalculator.CalculateCRM(_currentIndex, elapsed);

            if (elapsed.TotalSeconds >= CpmDisplayThresholdSeconds
                && _currentIndex > 0
                && _totalPressedKeys > 1
                && elapsed.TotalSeconds >= MinElapsedSecondsForDisplay)
            {
                double measured = calculated;

                double alpha = elapsed.TotalSeconds < MinElapsedSecondsForStable
                    ? 0.08 
                    : CpmSmoothingAlpha;

                if (elapsed.TotalSeconds < MinElapsedSecondsForStable)
                {
                    measured = Math.Min(measured, MaxCpmInitial);
                }

                if (_cpmSmoothed <= 0.0)
                {
                    _cpmSmoothed = measured;
                }
                else
                {
                    _cpmSmoothed = _cpmSmoothed * (1.0 - alpha) + measured * alpha;
                }

                Cpm = (int)Math.Round(_cpmSmoothed);
            }
            else
            {
                _cpmSmoothed = 0.0;
                Cpm = 0;
            }
        }
        else
        {
            _cpmSmoothed = 0.0;
            Cpm = 0;
        }

        if (_currentIndex == Characters.Count)
        {
            FinalCpm = Cpm;
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
        _totalPressedKeys = 0;
        IsExerciseFinished = false;
        IsLessonFinished = false;
        _currentIndex = 0;

        Errors = 0;
        Accurancy = 100;

        _exerciseStartTime = null;
        Cpm = 0;
        _cpmSmoothed = 0.0;

        _lastInputTime = null;
        _lastDecayUpdate = DateTime.UtcNow;

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
        _totalPressedKeys = 0;
        _currentExercise = Lesson.Exercises[index + 1];
        _currentIndex = 0;
        Errors = 0;
        Accurancy = 100;
        _exerciseStartTime = null;
        Cpm = 0;
        _cpmSmoothed = 0.0;

        _lastInputTime = null;
        _lastDecayUpdate = DateTime.UtcNow;

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
        _totalPressedKeys = 0;
        Accurancy = 100;
        Errors = 0;

        _exerciseStartTime = null;
        Cpm = 0;
        _cpmSmoothed = 0.0;

        _lastInputTime = null;
        _lastDecayUpdate = DateTime.UtcNow;

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
