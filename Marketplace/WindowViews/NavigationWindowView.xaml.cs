using System.Collections.ObjectModel;
using System.Windows.Controls;
using Marketplace.Pages;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Media;

namespace Marketplace.WindowViews;

[ObservableObject]
public partial class NavigationWindowView : UserControl
{
    #region Properties
    public ObservableCollection<INavigationControl> NavItems { get; private set; }

    public ObservableCollection<INavigationControl> FooterNavItems { get; private set; }

    [ObservableProperty]
    private string _currentPageTitle;

    [ObservableProperty]
    private string _title;
    #endregion

    public NavigationWindowView()
    {
        Title = "Маркетплэйс";

        InitializeComponent();
        InitiNavigationItems();

        DataContext = this;

        NavigationSideBar.PageService = App.PageService;
        App.NavigationService = NavigationSideBar;

        NavigationSideBar.Navigated += (_, _) =>
            CurrentPageTitle = $"{NavigationSideBar.Current!.Content}";
    }

    private void InitiNavigationItems()
    {
        var basketNavItem = new NavigationItem
        {
            Content = "Корзина",
            Icon = Wpf.Ui.Common.SymbolRegular.Cart24,
            PageType = typeof(BasketPage),
        };

        var basketNavItemDefaultBrush = basketNavItem.IconForeground;
        App.BasketService.StateChanged += () =>
        {
            if (App.BasketService.TotalItemsCount > 0)
            {
                basketNavItem.IconForeground = Brushes.Red;
                basketNavItem.Content = $"Корзина ({App.BasketService.TotalItemsCount})";
            }
            else
            {
                basketNavItem.IconForeground = basketNavItemDefaultBrush;
                basketNavItem.Content = "Корзина";
            }

            if (NavigationSideBar.Current == basketNavItem)
                CurrentPageTitle = $"{basketNavItem.Content}";
        };

        NavItems = new ObservableCollection<INavigationControl>
        {
            new NavigationItem
            {
                Content = "Личный кабинет",
                Icon = Wpf.Ui.Common.SymbolRegular.Person24,
                PageType = typeof(UserPage)
            },
            new NavigationItem
            {
                Content = "Товары",
                Icon = Wpf.Ui.Common.SymbolRegular.Box24,
                PageType = typeof(ProductsPage)
            },
            basketNavItem
        };
    }
}