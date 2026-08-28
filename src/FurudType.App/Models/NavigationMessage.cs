using CommunityToolkit.Mvvm.Messaging.Messages;

using FurudType.App.ViewModels;

namespace FurudType.App.Models;

public sealed class NavigationMessage<TPage> : ValueChangedMessage<object?>
    where TPage : ViewModelBase
{
    public NavigationMessage(object? parameter = null) : base(parameter)
    {
    }
}
