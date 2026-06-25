using System.Threading.Tasks;

using Avalonia.Controls;

using FurudType.App.ViewModels;

namespace FurudType.App.Views;

public partial class MainView : Window
{
    public MainView()
    {
        InitializeComponent();
    }

    private async void HandlePageLoaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (DataContext is MainViewModel viewModel)
        {
            await viewModel.LoadLessonsAsync();
        }
    }
}
