using Marketplace.Services;
using Wpf.Ui.Controls;
using Wpf.Ui.Controls.Interfaces;
using Marketplace.Pages;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace Marketplace.WindowViews;

public partial class NavigationWindowView : UserControl
{
    #region [ Properties ]
    public ObservableCollection<INavigationControl> NavigationItems { get; private set; }
    public ObservableCollection<INavigationControl> FooterNavigationItems { get; private set; }

    public INavigation Navigation { get; private set; }
    #endregion

    public NavigationWindowView()
    {
        InitializeComponent();

        DataContext = this;

        Navigation = NavigationSideBar;
        Navigation.PageService = new NavigationPageService();

        InitiNavigationItems();
    }

    private void InitiNavigationItems()
    {
        NavigationItems = new ObservableCollection<INavigationControl>
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

        FooterNavigationItems = new ObservableCollection<INavigationControl>
        {
            new NavigationItem
            {
                Content = "Корзина",
                Icon = Wpf.Ui.Common.SymbolRegular.PaintBucket24,
                PageType = typeof(BasketPage)
            },
            new NavigationItem
            {
                Content = "Личный кабинет",
                Icon = Wpf.Ui.Common.SymbolRegular.Person24,
                PageType = typeof(UserPage)
            }
        };
    }
}