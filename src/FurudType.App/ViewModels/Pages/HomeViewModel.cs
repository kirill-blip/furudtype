using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

using FurudType.App.Models;

namespace FurudType.App.ViewModels.Pages;

public partial class HomeViewModel : ViewModelBase
{
    private readonly IMessenger _messenger;

    public HomeViewModel(IMessenger messenger)
    {
        _messenger = messenger;
    }

    [RelayCommand]
    private void GoToCourse()
    {
        _messenger.Send(new NavigationMessage<LessonViewModel>());
    }
}
