using System;
using System.Globalization;
using System.Windows.Data;
using Marketplace.DataTypes.Enums;

namespace Marketplace.Converters;

public class ProductStatusToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is ProductStatus productStatus == false)
            return null!;

        return productStatus switch
        {
            ProductStatus.None => "Любой",
            ProductStatus.Active => "В продаже",
            ProductStatus.RemovedFromSale => "Снят с продажи",
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
