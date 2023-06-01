using System;
using System.Data;
using System.Globalization;
using System.Windows.Data;
using Marketplace.DataTypes.Enums;

namespace Marketplace.Converters;

public class UserRoleToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is UserRole userRole == false)
            return null;

        return userRole switch
        {
            UserRole.Client => "Покупатель",
            UserRole.Salesman => "Продавец",
            UserRole.Employee => "Работник",
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
