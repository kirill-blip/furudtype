using Avalonia.Controls;
using FurudType.App.ViewModels;
using FurudType.Storage;

namespace FurudType.App.Views;

public partial class MainView : Window
{
    public MainView()
    {
        InitializeComponent();
        DataContext = new MainViewModel(new InMemoryLessonRepository());
    }
}
