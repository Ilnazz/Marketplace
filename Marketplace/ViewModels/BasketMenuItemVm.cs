using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Marketplace.Pages;

namespace Marketplace.ViewModels;

public partial class BasketMenuItemVm : ObservableObject
{
    [ObservableProperty]
    private int _itemsCount;

    [RelayCommand]
    private void NavigateToBasketPage()
    {
        App.NavigationService.Navigate(typeof(BasketPage));
        UpdateNavWindowCurrentPageTitle();
    }

    public BasketMenuItemVm()
    {
        App.BasketService.StateChanged += () =>
        {
            ItemsCount = App.BasketService.TotalItemsCount;
            UpdateNavWindowCurrentPageTitle();
        };
    }

    private void UpdateNavWindowCurrentPageTitle() =>
            App.NavigationWindowVm.CurrentPageTitle = $"Корзина ({ItemsCount})";
}
