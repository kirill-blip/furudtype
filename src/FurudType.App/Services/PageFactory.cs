using System;

using FurudType.App.ViewModels;

namespace FurudType.App.Services;

public class PageFactory
{
    private readonly Func<Type, ViewModelBase> _factory;

    public PageFactory(Func<Type, ViewModelBase> factory)
    {
        _factory = factory;
    }

    public ViewModelBase GetPageViewModel<T>(Action<T>? afterCreation = null)
        where T : ViewModelBase
    {
        ViewModelBase viewModel = _factory(typeof(T));

        afterCreation?.Invoke((T)viewModel);

        return viewModel;
    }
}
