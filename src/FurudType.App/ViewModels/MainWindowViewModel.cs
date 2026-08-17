using System;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;

using FurudType.App.Models;
using FurudType.App.Services;
using FurudType.App.ViewModels.Pages;

namespace FurudType.App.ViewModels;

public partial class MainWindowViewModel : ViewModelBase, IDisposable
{
    private readonly PageFactory _pageFactory;
    private readonly IMessenger _messenger;

    public MainWindowViewModel(
        PageFactory pageFactory,
        IMessenger messenger)
    {
        _pageFactory = pageFactory;
        _messenger = messenger;

        _currentPage = _pageFactory.GetPageViewModel<HomeViewModel>();

        _messenger.Register<MainWindowViewModel, NavigationMessage<LessonViewModel>>(this, OpenLessonView);
    }

    public void Dispose()
    {
        _messenger.Unregister<NavigationMessage<LessonViewModel>>(this);
    }

    [ObservableProperty]
    private ViewModelBase _currentPage;

    private void OpenLessonView(MainWindowViewModel recipient, NavigationMessage<LessonViewModel> message)
    {
        CurrentPage = _pageFactory.GetPageViewModel<LessonViewModel>();
    }
}
