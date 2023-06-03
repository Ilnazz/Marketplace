using System.Collections.Generic;
using System.Linq;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Marketplace.Database;
using Marketplace.Models;
using Marketplace.Pages;
using Marketplace.WindowViewModels;
using Marketplace.WindowViews;

namespace Marketplace.PageViewModels;

public partial class BasketPageVm : PageVmBase
{
    #region Properties
    [ObservableProperty]
    private IEnumerable<ProductModel> _productModels;

    public int TotalProductsCount => ProductModels.Sum(pm => pm.QuantityInBasket);
    public bool IsEmpty => TotalProductsCount == 0;

    public bool IsThereDiscount => ProductModels.Any(pm => pm.HasDiscount);

    public decimal TotalProductsCost => ProductModels.Sum(pm => pm.TotalCost);
    public decimal TotalProductsCostWithDiscount => ProductModels.Sum(pm => pm.TotalCostWithDiscount);
    public decimal TotalDiscountSum => TotalProductsCost - TotalProductsCostWithDiscount;
    #endregion

    #region Commands
    [RelayCommand]
    private void NavigateToProductsPage() =>
        App.NavigationService.Navigate(typeof(BookProductsPage));


    [RelayCommand]
    private void ShowMakeOrderWindow()
    {
        if (App.UserService.IsUserAuthorized() == false)
        {
            var authWindowVm = new AuthWindowVm();
            var authWindowView = new AuthWindowView() { DataContext = authWindowVm };

            var dialogWindow = new Wpf.Ui.Controls.MessageBox
            {
                Content = authWindowView,
                Width = authWindowView.Width + 30,
                Height = authWindowView.Height,
                SizeToContent = SizeToContent.Height,
                ResizeMode = ResizeMode.NoResize,
                Title = authWindowVm.Title,
                ShowFooter = false,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            authWindowVm.CloseWindowMethod += dialogWindow.Close;
            dialogWindow.ShowDialog();

            if (App.UserService.IsUserAuthorized())
                new TitledContainerWindow(new MakeOrderWindowVm(ProductModels)).ShowDialog();
        }
        else
            new TitledContainerWindow(new MakeOrderWindowVm(ProductModels)).ShowDialog();
    }


    [RelayCommand]
    private void ClearBasket() =>
        App.BasketService.ClearBasket();
    #endregion

    public BasketPageVm()
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

        App.UserService.StateChanged += OnBasketServiceStateChanged;

        RefreshProductModels();
    }

    private void OnBasketServiceStateChanged()
    {
        RefreshProductModels();
        OnPropertyChanged(nameof(IsEmpty));
        OnPropertyChanged(nameof(TotalProductsCount));
        OnPropertyChanged(nameof(TotalProductsCostWithDiscount));
        OnPropertyChanged(nameof(TotalDiscountSum));
    }

    private void RefreshProductModels()
    {
        var prodAndCounts = App.BasketService.GetItemAndCounts();
        ProductModels = prodAndCounts.Select(pac => new ProductModel(pac.Key));
    }
}
