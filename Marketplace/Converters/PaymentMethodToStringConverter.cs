using System;
using System.Data;
using System.Globalization;
using System.Windows.Data;
using Marketplace.DataTypes.Enums;

namespace Marketplace.Converters;

public class PaymentMethodToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is PaymentMethod paymentMethod  == false)
            return null;

        return paymentMethod switch
        {
            PaymentMethod.None => "Любой",
            PaymentMethod.InCash => "Наличными при получении",
            PaymentMethod.ByBankCard => "Банковской картой онлайн"
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
