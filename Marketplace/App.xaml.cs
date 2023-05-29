using System.Windows;
using Marketplace.Database.Models;
using Marketplace.Pages;
using Marketplace.Services;
using Marketplace.WindowViewModels;
using Marketplace.WindowViews;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm.Contracts;

namespace Marketplace;

public partial class App : Application
{
    public static User? CurrentUser { get; set; }

    public static AuthRegService AuthRegService { get; set; }

    public static IPageService PageService { get; set; }

    public static INavigation NavigationService { get; set; }
    public static Window NavigationWindow { get; set; }

    public static IBasketService<Product> BasketService { get; set; }

    protected override void OnStartup(StartupEventArgs e)
    {
        PageService = new PageService();
        AuthRegService = new AuthRegService();
        BasketService = new GuestBasketService<Product>();

        NavigationWindow = new ContainerWindow(new NavigationWindowVm());
        NavigationWindow.Show();
        NavigationService.Navigate(typeof(ProductsPage));
    }
}
