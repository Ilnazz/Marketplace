using CommunityToolkit.Mvvm.Input;
using Marketplace.Database.Models;
using Marketplace.Pages;

namespace Marketplace.PageViewModels;

public partial class OrdersPageVm : PageVmBase
{
    public bool AreThereOrders => ((Client)App.UserService.CurrentUser).Orders.Count > 0;

    [RelayCommand]
    private void NavigateToProductsPage() =>
        App.NavigationService.Navigate(typeof(BookProductsPage));

    public OrdersPageVm()
    {
    }
}