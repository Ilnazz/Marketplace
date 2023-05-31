using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Marketplace.Pages;

namespace Marketplace.ViewModels;

public partial class BasketMenuItemVm : ObservableObject
{
    [ObservableProperty]
    private int _itemsCount;

    public bool IsEmpty => _itemsCount == 0;

    [RelayCommand]
    private void NavigateToBasketPage()
    {
        App.NavigationService.Navigate(typeof(BasketPage));
        UpdateNavWindowCurrentPageTitle();
    }

    [RelayCommand]
    private void ClearBasket()
    {
        App.BasketService.ClearBasket();
    }

    public BasketMenuItemVm()
    {
        App.BasketService.StateChanged += () =>
        {
            ItemsCount = App.BasketService.TotalItemsCount;
            UpdateNavWindowCurrentPageTitle();
            OnPropertyChanged(nameof(IsEmpty));
        };
    }

    private void UpdateNavWindowCurrentPageTitle() =>
            App.NavigationWindowVm.CurrentPageTitle = $"Корзина ({ItemsCount})";
}
