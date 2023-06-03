using CommunityToolkit.Mvvm.Input;
using Marketplace.Database;
using Marketplace.Database.Models;
using Marketplace.Pages;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.PageViewModels;

public partial class OrdersPageVm : PageVmBase
{
    public bool AreThereOrders => ((Client)App.UserService.CurrentUser).Orders.Count > 0;

    [RelayCommand]
    private void NavigateToProductsPage() =>
        App.NavigationService.Navigate(typeof(BookProductsPage));

    public OrdersPageVm()
    {
        DatabaseContext.Entities.Orders.Load();
    }
}