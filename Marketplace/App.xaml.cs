using System.Windows;
using Marketplace.Database.Models;
using Marketplace.Pages;
using Marketplace.Services;
using Marketplace.WindowViewModels;
using Marketplace.WindowViews;
using Wpf.Ui.Controls.Interfaces;

namespace Marketplace;

public partial class App : Application
{
    public static UserService UserService { get; set; }

    public static INavigation NavigationService { get; set; }
    public static NavigationWindowVm NavigationWindowVm { get; set; }

    public static IBasketService<Product> BasketService { get; set; }

    public static SearchService SearchService { get; set; }

    protected override void OnStartup(StartupEventArgs e)
    {
        App.UserService = new UserService();
        App.BasketService = new GuestBasketService<Product>();

        NavigationWindowVm = new NavigationWindowVm();
        new ContainerWindow(NavigationWindowVm).Show();

        App.NavigationService.Navigate(typeof(BookProductsPage));
    }
}
