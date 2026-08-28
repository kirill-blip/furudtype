using Avalonia.Controls;

using FurudType.App.ViewModels.Pages;

namespace FurudType.App.Views.Pages;

public partial class LessonView : UserControl
{
    public LessonView()
    {
        InitializeComponent();
    }

    private async void HandlePageLoaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (DataContext is LessonViewModel viewModel)
        {
            await viewModel.LoadLessonsAsync();
        }
    }
}
