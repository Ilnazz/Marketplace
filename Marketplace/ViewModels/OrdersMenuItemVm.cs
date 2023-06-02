using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Marketplace.Pages;

namespace Marketplace.ViewModels;

public partial class OrdersMenuItemVm : ObservableObject
{
    public bool IsUserAuthorized => App.UserService.IsUserAuthorized();

    [RelayCommand]
    private void NavigateToOrdersPage()
    {
        App.NavigationService.Navigate(typeof(OrdersPage));
        UpdateNavWindowCurrentPageTitle();
    }
    private void UpdateNavWindowCurrentPageTitle() =>
        App.NavigationWindowVm.CurrentPageTitle = $"Заказы";

    public OrdersMenuItemVm()
    {
        App.UserService.StateChanged += () => OnPropertyChanged(nameof(IsUserAuthorized));
    }
}
