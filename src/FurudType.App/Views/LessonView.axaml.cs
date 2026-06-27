using Avalonia.Controls;

using FurudType.App.ViewModels;

namespace FurudType.App.Views;

public partial class LessonView : UserControl
{
    public LessonView()
    {
        InitializeComponent();
    }

    private async void HandlePageLoaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (DataContext is LesssonViewModel viewModel)
        {
            await viewModel.LoadLessonsAsync();
        }
    }
}
