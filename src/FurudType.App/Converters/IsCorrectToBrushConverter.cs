using System;
using System.Globalization;

using Avalonia.Data.Converters;
using Avalonia.Media;

namespace FurudType.App.Converters;

public class IsCorrectToBrushConverter : IValueConverter
{
    private IBrush CorrectBrush { get; } = Brushes.LimeGreen;
    private IBrush IncorrectBrush { get; } = Brushes.Red;
    private IBrush DefaultBrush { get; } = Brushes.Black;

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value switch
        {
            true => CorrectBrush,
            false => IncorrectBrush,
            _ => DefaultBrush
        };
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
