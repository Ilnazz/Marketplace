using System.Windows;
using Marketplace.Database.Models;
using Marketplace.Services;
using Marketplace.WindowViewModels;
using Marketplace.WindowViews;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm.Contracts;

namespace Marketplace;

public partial class App : Application
{
    public static App Instance { get; private set; }

    public User? CurrentUser { get; set; }

    public IPageService PageService { get; private set; }

    public INavigation NavigationService { get; set; }

    public IBasketService<Product> BasketService { get; set; }

    public App() => Instance = this;

    protected override void OnStartup(StartupEventArgs e)
    {
        PageService = new PageService();
        new ContainerWindow(new NavigationWindowVm()).Show();
    }
}
