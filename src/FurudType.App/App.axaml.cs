using System;
using System.IO;

using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

using CommunityToolkit.Mvvm.Messaging;

using FurudType.App.Services;
using FurudType.App.ViewModels;
using FurudType.App.ViewModels.Pages;
using FurudType.App.Views;
using FurudType.App.Views.Pages;
using FurudType.Core;
using FurudType.Core.Repositories;
using FurudType.Storage;
using FurudType.Storage.Repositories;

using Microsoft.Extensions.DependencyInjection;

namespace FurudType.App;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);

    }

    public override void OnFrameworkInitializationCompleted()
    {
        StorageSettings storageSettings = new StorageSettings()
        {
            DataPath = Path.Combine(AppContext.BaseDirectory, "Data", "en"),
        };

        ServiceCollection collection = new();

        collection.AddScoped<MainWindowViewModel>();
        collection.AddScoped<KeyboardViewModel>();
        collection.AddScoped<LessonViewModel>();
        collection.AddScoped<HomeViewModel>();
        collection.AddScoped<MetricsCalculator>();

        collection.AddScoped<HomeView>();
        collection.AddScoped<LessonView>();

        collection.AddScoped((x) => storageSettings);

        collection.AddSingleton<PageFactory>();
        collection.AddSingleton<IMessenger>(WeakReferenceMessenger.Default);
        collection.AddSingleton<Func<Type, ViewModelBase>>(x => type => type switch
        {
            _ when type == typeof(HomeViewModel) => x.GetRequiredService<HomeViewModel>(),
            _ when type == typeof(LessonViewModel) => x.GetRequiredService<LessonViewModel>(),
            _ => throw new InvalidOperationException($"Page of type {type?.FullName} has no view model"),
        });

        collection.AddScoped<ILessonRepository, JsonLessonRepository>();

        ServiceProvider services = collection.BuildServiceProvider();
        DataTemplates.Add(new ViewLocator(services));

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow()
            {
                DataContext = services.GetRequiredService<MainWindowViewModel>()
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
