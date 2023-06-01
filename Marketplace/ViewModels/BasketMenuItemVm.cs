using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Marketplace.Pages;

namespace Marketplace.ViewModels;

public partial class BasketMenuItemVm : ObservableObject
{
    [ObservableProperty]
    private int _itemsCount;

    public bool IsEmpty => ItemsCount == 0;

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
        App.BasketService.StateChanged += OnBasketServiceStateChanged;
        App.BasketServiceProviderChanging += () =>
        {
            App.BasketService.StateChanged -= OnBasketServiceStateChanged;
        };
        App.BasketServiceProviderChanged += () =>
        {
            App.BasketService.StateChanged += OnBasketServiceStateChanged;
            OnBasketServiceStateChanged();
        };
    }

    private void OnBasketServiceStateChanged()
    {
        ItemsCount = App.BasketService.TotalItemsCount;

        if (App.NavigationWindowVm.CurrentPageTitle.StartsWith("Корзина"))
            UpdateNavWindowCurrentPageTitle();

        OnPropertyChanged(nameof(IsEmpty));
    }

    private void UpdateNavWindowCurrentPageTitle() =>
        App.NavigationWindowVm.CurrentPageTitle = $"Корзина ({ItemsCount})";
}
