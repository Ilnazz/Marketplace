    using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Marketplace.Converters;

public class BoolToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue == false)
            throw new ArgumentException("Expected argument of type bool", nameof(value));

        if (parameter is bool targetBoolValue)
            boolValue = boolValue == targetBoolValue;

        return boolValue ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Visibility visibility == false)
            throw new ArgumentException("Expected argument of type Visibility", nameof(value));

        return visibility == Visibility.Visible ? true : false;
    }
}
