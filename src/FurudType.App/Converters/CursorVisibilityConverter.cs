using System;
using System.Globalization;

using Avalonia.Data.Converters;
using Avalonia.Media;

namespace FurudType.App.Converters;

public class CursorVisibilityConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null)
        {
            return null;
        }

        bool visible = (bool)value;

        return visible ? TextDecorations.Underline : null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
