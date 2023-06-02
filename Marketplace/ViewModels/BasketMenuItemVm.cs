using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Marketplace.Pages;

namespace Marketplace.ViewModels;

public partial class BasketMenuItemVm : ObservableObject
{
    #region Properties
    [ObservableProperty]
    private int _itemsCount;

    public bool IsEmpty => ItemsCount == 0;
    #endregion

    #region Commands
    [RelayCommand]
    private void NavigateToBasketPage()
    {
        if (App.NavigationWindowVm.CurrentPageTitle?.StartsWith("Корзина") == false)
        {
            App.NavigationService.Navigate(typeof(BasketPage));
            UpdateNavWindowCurrentPageTitle();
        }
    }
    #endregion

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
