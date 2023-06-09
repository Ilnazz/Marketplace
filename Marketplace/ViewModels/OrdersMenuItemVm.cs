using System.Linq;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Marketplace.DataTypes.Enums;
using Marketplace.Pages;

namespace Marketplace.ViewModels;

public partial class OrdersMenuItemVm : ObservableObject
{
    public bool UserHasPermission =>
         App.UserService.CurrentUser.Permissions.Contains(Permission.ViewOrdersPage);

    private string _title => App.UserService.CurrentUser.Role != UserRole.Client ? "Заказы покупателей" : "Заказы";

    [RelayCommand]
    private void NavigateToOrdersPage()
    {
        if (App.NavigationWindowVm.CurrentPageTitle?.Equals(_title) == false)
        {
            App.NavigationService.Navigate(typeof(OrdersPage));
            UpdateNavWindowCurrentPageTitle();
        }

        App.SearchService.IsEnabled = false;
    }
    private void UpdateNavWindowCurrentPageTitle() =>
        App.NavigationWindowVm.CurrentPageTitle = _title;

    public OrdersMenuItemVm()
    {
        App.UserService.StateChanged += () => OnPropertyChanged(nameof(UserHasPermission));
    }
}
