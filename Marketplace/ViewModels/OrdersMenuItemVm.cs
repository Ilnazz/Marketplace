using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Marketplace.Pages;

namespace Marketplace.ViewModels;

public partial class OrdersMenuItemVm : ObservableObject
{
    [RelayCommand]
    private void NavigateToOrdersPage()
    {
        App.NavigationService.Navigate(typeof(OrdersPage));
        UpdateNavWindowCurrentPageTitle();
    }
    private void UpdateNavWindowCurrentPageTitle() =>
            App.NavigationWindowVm.CurrentPageTitle = $"Заказы";
}
