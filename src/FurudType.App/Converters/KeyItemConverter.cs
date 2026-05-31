using System;
using System.Globalization;

using Avalonia.Data.Converters;
using Avalonia.Media;

namespace FurudType.App.Converters;

public class KeyItemConverter : IValueConverter
{
    private IBrush CurrentKeyBrush { get; } = Brushes.LimeGreen;
    private IBrush DefaultKeyBrush { get; } = Brushes.AliceBlue;

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value switch
        {
            true => CurrentKeyBrush,
            false => DefaultKeyBrush,
            _ => null
        };
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
