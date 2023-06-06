using System;
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
            OrderStatus.InProcessing => "В обработке",
            OrderStatus.Canceled => "Отменён",
            OrderStatus.Delivered => "Доставлен",
            OrderStatus.Completed => "Завершён",
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
