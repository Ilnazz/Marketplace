using System;
using System.Windows;
using Marketplace.Pages;
using Wpf.Ui.Mvvm.Contracts;

namespace Marketplace.Services;

public class NavigationPageService : IPageService
{
    public FrameworkElement? GetPage(Type pageType)
    {
        if (pageType == typeof(UserPage))
            return new UserPage();
        else if (pageType == typeof(ProductsPage))
            return new ProductsPage();
        else if (pageType == typeof(BasketPage))
            return new BasketPage();
        else if (pageType == typeof(OrdersPage))
            return new OrdersPage();

        return null;
    }
    public T? GetPage<T>() where T : class
        => (T)Activator.CreateInstance(typeof(T))!;
}
