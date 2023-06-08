using System;
using System.Globalization;
using System.Windows.Data;
using Marketplace.PageViewModels;

namespace Marketplace.Converters;

public class ReportTypeToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is ReportType rp == false)
            return null;

        return rp switch
        {
            ReportType.ByOrdersCount => "По количеству заказов",
            ReportType.ByProductsCount => "По количеству товаров в заказе",
            ReportType.ByTotalCost => "По общей стоимости заказа"
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
