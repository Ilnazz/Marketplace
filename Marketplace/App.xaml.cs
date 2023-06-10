using System;
using System.Threading.Tasks;
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


    public static event Action? BasketServiceProviderChanging,
                                BasketServiceProviderChanged;
    private static IBasketService<Product> _basketService = null!;
    public static IBasketService<Product> BasketService
    {
        get => _basketService;
        set
        {
            BasketServiceProviderChanging?.Invoke();
            _basketService = value;
            BasketServiceProviderChanged?.Invoke();
        }
    }

    public static SearchService SearchService { get; set; }

    protected override void OnStartup(StartupEventArgs e)
    {
        App.UserService = new UserService();
        App.BasketService = new GuestBasketService<Product>();

        NavigationWindowVm = new NavigationWindowVm();
        new ContainerWindow(NavigationWindowVm).Show();

        App.NavigationService.Navigate(typeof(BookProductsPage));
    }

    public App()
    {
        AppDomain.CurrentDomain.UnhandledException += (_, e) => { };
        Dispatcher.UnhandledException += (_, e) => { e.Handled = true; };
        Current.DispatcherUnhandledException += (_, e) => { e.Handled = true; };
        TaskScheduler.UnobservedTaskException += (_, e) => { e.SetObserved(); };
    }
}
