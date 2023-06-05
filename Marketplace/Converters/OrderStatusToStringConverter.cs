using System;
using System.Data;
using System.Globalization;
using System.Windows.Data;
using Marketplace.DataTypes.Enums;

namespace Marketplace.Converters;

public class OrderStatusToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is OrderStatus orderStatus == false)
            return null;

        return orderStatus switch
        {
            OrderStatus.None => "Любой",
            OrderStatus.Created => "Создан",
            OrderStatus.Processing => "В обработке",
            OrderStatus.Completed => "Завершён",
            OrderStatus.Rejected => "Отклонён"
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
