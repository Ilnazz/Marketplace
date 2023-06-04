using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using Marketplace.DataTypes.Enums;
using Marketplace.Services;

namespace Marketplace.Converters;

public class PermissionToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Permission permission == false)
            throw new ArgumentException("Should be a value from Permission enumeration", nameof(value));

        return App.UserService.CurrentUser.Permissions.Contains(permission)
            ? Visibility.Visible
            : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
