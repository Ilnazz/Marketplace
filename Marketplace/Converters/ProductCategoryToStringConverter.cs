using System;
using System.Globalization;
using System.Windows.Data;
using Marketplace.DataTypes.Enums;

namespace Marketplace.Converters;

public class ProductCategoryToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is ProductCategory productCategory == false)
            return null!;

        return productCategory switch
        {
            ProductCategory.Electronics => "Электроника",
            ProductCategory.ClothesAndShoes => "Одежда и обувь",
            ProductCategory.HouseAndGarden => "Дом и сад",
            ProductCategory.Children => "Детские товары",
            ProductCategory.BeautyAndHealth => "Красота и здоровье",
            ProductCategory.HomeAppliances => "Бытовая техника",
            ProductCategory.SportsAndRecreation => "Спорт и отдых",
            ProductCategory.ConstructionAndRepair => "Строительство и ремонт",
            ProductCategory.Food => "Продукты питания",
            ProductCategory.Pharmacy => "Аптека",
            ProductCategory.Pet => "Товары для животных",
            ProductCategory.Books => "Книги",
            ProductCategory.TourismFishingHunting => "Туризм, рыбалка, охота",
            ProductCategory.Automotive => "Автотовары",
            ProductCategory.Furniture => "Мебель",
            ProductCategory.HobbyAndCreativity => "Хобби и творчество",
            ProductCategory.Jewelry => "Ювелирные украшения",
            ProductCategory.GameAndConsoles => "Игры и консоли",
            ProductCategory.OfficeSupplies => "Канцелярские товары",
            ProductCategory.HouseholdChemicalsAndHygiene => "Бытовая химия и гигиента"
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
