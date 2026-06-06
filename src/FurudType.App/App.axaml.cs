using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

using FurudType.App.ViewModels;
using FurudType.App.Views;
using FurudType.Core;
using FurudType.Core.Repositories;
using FurudType.Storage;

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
        var collection = new ServiceCollection();

        collection.AddScoped<MainViewModel>();
        collection.AddScoped<KeyboardViewModel>();
        collection.AddScoped<MetricsCalculator>();

        collection.AddScoped<ILessonRepository, InMemoryLessonRepository>();

        var services = collection.BuildServiceProvider();

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
