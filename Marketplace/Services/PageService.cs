using System;
using System.Windows;
using Wpf.Ui.Mvvm.Contracts;

namespace Marketplace.Services;

public class PageService : IPageService
{
    public FrameworkElement? GetPage(Type pageType)
    {
        var pageInstance = Activator.CreateInstance(pageType);
        if (pageInstance is FrameworkElement frameworkElement == false)
            throw new ArgumentException("Should be framework element", nameof(pageType));

        return frameworkElement;
    }
    public T? GetPage<T>() where T : class
        => (T)Activator.CreateInstance(typeof(T))!;
}