using Avalonia.Controls;
using Avalonia.Controls.Templates;

using FurudType.App.ViewModels;

namespace FurudType.App;

public class ViewLocator : IDataTemplate
{
    public Control? Build(object? param)
    {
        return new HomePageView();
    }

    public bool Match(object? data)
    {
        return data is ViewModelBase;
    }
}
