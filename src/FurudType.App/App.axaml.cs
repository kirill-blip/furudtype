using System;
using System.IO;

using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

using FurudType.App.ViewModels;
using FurudType.App.Views;
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

        DataTemplates.Add(new ViewLocator());
    }

    public override void OnFrameworkInitializationCompleted()
    {
        StorageSettings storageSettings = new StorageSettings()
        {
            DataPath = Path.Combine(AppContext.BaseDirectory, "Data", "en"),
        };

        ServiceCollection collection = new();

        collection.AddScoped<MainViewModel>();
        collection.AddScoped<KeyboardViewModel>();
        collection.AddScoped<MetricsCalculator>();
        collection.AddScoped<LesssonViewModel>();
        collection.AddScoped((x) => storageSettings);

        collection.AddScoped<ILessonRepository, JsonLessonRepository>();

        ServiceProvider services = collection.BuildServiceProvider();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainView()
            {
                DataContext = services.GetRequiredService<MainViewModel>()
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
