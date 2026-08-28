using System;

using Avalonia.Controls;
using Avalonia.Controls.Templates;

using FurudType.App.ViewModels;
using FurudType.App.ViewModels.Pages;
using FurudType.App.Views;
using FurudType.App.Views.Pages;

namespace FurudType.App;

public class ViewLocator : IDataTemplate
{
    private readonly IServiceProvider _serviceProvider;

    public ViewLocator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Control? Build(object? data)
    {
        return data switch
        {
            MainWindowViewModel => new MainWindow(),
            HomeViewModel => new HomeView(),
            LessonViewModel => new LessonView(),
            _ => new TextBlock { Text = $"No view for {data?.GetType().Name}" },
        };
    }

    public bool Match(object? data)
    {
        return data is ViewModelBase;
    }
}
