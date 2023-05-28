using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using Marketplace.Pages;
using Wpf.Ui.Controls;
using Wpf.Ui.Controls.Interfaces;

namespace Marketplace.WindowViewModels;

public partial class NavigationWindowVm : WindowVmBase
{
    #region Properties
    public ObservableCollection<INavigationControl> NavItems { get; private set; }

    public ObservableCollection<INavigationControl> FooterNavItems { get; private set; }

    #endregion

    private static readonly BrushConverter _brushConverter = new();

    public NavigationWindowVm()
    {
        InitiNavigationItems();
    }

    private void InitiNavigationItems()
    {
        NavItems = new ObservableCollection<INavigationControl>
        {
            new NavigationItem
            {
                Content = "Товары",
                Icon = Wpf.Ui.Common.SymbolRegular.Box24,
                PageType = typeof(ProductsPage)
            },
            new NavigationItem
            {
                Content = "Заказы",
                Icon = Wpf.Ui.Common.SymbolRegular.ReOrder24,
                PageType = typeof(OrdersPage)
            }
        };

        var basketNavItem = new NavigationItem
        {
            Content = "Корзина",
            Icon = Wpf.Ui.Common.SymbolRegular.PaintBucket24,
            PageType = typeof(BasketPage),
        };

        var basketNavItemDefaultBrush = basketNavItem.IconForeground;

        App.BasketService.StateChanged += () =>
        {
            if (App.BasketService.TotalItemsCount > 0)
            {
                basketNavItem.IconForeground = Brushes.Red;
                basketNavItem.Content = $"В корзине: {App.BasketService.TotalItemsCount}";
            }
            else
            {
                basketNavItem.IconForeground = basketNavItemDefaultBrush;
                basketNavItem.Content = "Корзина";
            }
        };

        FooterNavItems = new ObservableCollection<INavigationControl>
        {
            basketNavItem,
            new NavigationItem
            {
                Content = "Личный кабинет",
                Icon = Wpf.Ui.Common.SymbolRegular.Person24,
                PageType = typeof(UserPage)
            }
        };
    }
}
