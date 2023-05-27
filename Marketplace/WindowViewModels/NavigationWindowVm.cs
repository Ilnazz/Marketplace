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

    [ObservableProperty]
    private Brush _basketNavItemForeground;
    #endregion

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

        var basketNavigationItem = new NavigationItem
        {
            Content = "Корзина",
            Icon = Wpf.Ui.Common.SymbolRegular.PaintBucket24,
            PageType = typeof(BasketPage),
        };
        basketNavigationItem.SetBinding(NavigationItem.IconForegroundProperty, new Binding(nameof(BasketNavItemForeground))
        {
            Mode = BindingMode.OneWay,
        });

        App.Instance.BasketService.StateChanged += () =>
        {
            if (App.Instance.BasketService.ItemCount > 0)
                BasketNavItemForeground = Brushes.Red;
            else
                BasketNavItemForeground = Brushes.White;
        };

        FooterNavItems = new ObservableCollection<INavigationControl>
        {
            basketNavigationItem,
            new NavigationItem
            {
                Content = "Личный кабинет",
                Icon = Wpf.Ui.Common.SymbolRegular.Person24,
                PageType = typeof(UserPage)
            }
        };
    }
}
