using System.Collections.Generic;
using System.Linq;

using CommunityToolkit.Mvvm.ComponentModel;

using FurudType.App.ViewModels.Keyboard;

namespace FurudType.App.ViewModels;

public partial class KeyboardViewModel : ViewModelBase
{
    private readonly string[] _layoutRows = [
        "1234567890-=",
        "QWERTYUIOP[]",
        "ASDFGHJKL;'",
        "ZXCVBNM,./",
        " "
    ];

    [ObservableProperty]
    private List<KeyItemViewModel[]> _keys = [];

    private KeyItemViewModel? _currentKey = null;

    public KeyboardViewModel()
    {
        var generatedRows = new List<KeyItemViewModel[]>();

        foreach (var rowText in _layoutRows)
        {
            var rowKeys = rowText.Select(ch => new KeyItemViewModel
            {
                DisplayText = ch.ToString().ToUpper(),
            }).ToArray();

            generatedRows.Add(rowKeys);
        }

        Keys = generatedRows;
    }

    private KeyItemViewModel? FindKeyItem(string key)
    {
        foreach (KeyItemViewModel[] keyItems in Keys)
        {
            foreach (KeyItemViewModel item in keyItems)
            {
                if (item.DisplayText.ToLower() == key.ToLower())
                {
                    return item;
                }
            }
        }

        return null;
    }

    public void ChangeKeyItem(char key)
    {
        _currentKey?.IsCurrent = false;

        KeyItemViewModel? keyItem = FindKeyItem(key.ToString());

        if (keyItem != null)
        {
            _currentKey = keyItem;
            _currentKey.IsCurrent = true;
        }
    }
}
