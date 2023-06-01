using Marketplace.DataTypes.Enums;
using System.Globalization;
using System.Windows.Data;
using System;
using Wpf.Ui.Common;

namespace Marketplace.Converters;

public class UserRoleToSymbolIconConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is UserRole userRole == false)
            return null;

        return userRole switch
        {
            UserRole.Client => SymbolRegular.Backpack12,
            UserRole.Salesman => SymbolRegular.Box16,
            UserRole.Employee => SymbolRegular.Guardian20,
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}