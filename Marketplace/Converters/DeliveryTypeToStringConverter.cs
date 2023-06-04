using System;
using System.Data;
using System.Globalization;
using System.Windows.Data;
using Marketplace.DataTypes.Enums;

namespace Marketplace.Converters;

public class DeliveryTypeToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is DeliveryType deliveryType == false)
            return null;

        return deliveryType switch
        {
            DeliveryType.Pickup => "Самовывоз",
            DeliveryType.ToHome => "На дом",
            DeliveryType.ToDeliveryPoint => "В пункт выдачи",
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
