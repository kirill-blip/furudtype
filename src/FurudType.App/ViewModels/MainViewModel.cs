using CommunityToolkit.Mvvm.ComponentModel;

namespace FurudType.App.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    private ViewModelBase _currentPage;

    public MainViewModel(LesssonViewModel lesssonViewModel)
    {
        _currentPage = lesssonViewModel;
    }
}
