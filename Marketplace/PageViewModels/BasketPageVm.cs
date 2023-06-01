using System.Collections.Generic;
using System.Linq;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Marketplace.Database;
using Marketplace.Pages;
using Marketplace.WindowViewModels;
using Marketplace.WindowViews;

namespace Marketplace.PageViewModels;

public partial class BasketPageVm : PageVmBase
{
    #region Properties
    [ObservableProperty]
    private IEnumerable<ProductModel> _productModels;

    public bool IsEmpty => TotalProductsCount == 0;

    public int TotalProductsCount => ProductModels.Sum(pm => pm.QuantityInBasket);

    public bool IsThereDiscount => ProductModels.Any(pm => pm.HasDiscount);

    public decimal TotalProductsCost => ProductModels.Sum(pm => pm.TotalCost);
    public decimal TotalProductsCostWithDiscount => ProductModels.Sum(pm => pm.TotalCostWithDiscount);
    public decimal TotalDiscountSum => TotalProductsCost - TotalProductsCostWithDiscount;
    #endregion

    [RelayCommand]
    private void GoToProductsPage() =>
        App.NavigationService.Navigate(typeof(BookProductsPage));

    [RelayCommand]
    private void MakeOrder()
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
                new TitledContainerWindow(new MakeOrderWindowVm()).ShowDialog();
        }
        else
            new TitledContainerWindow(new MakeOrderWindowVm()).ShowDialog();
    }

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

        var prodAndCounts = App.BasketService.GetItemAndCounts();
        ProductModels = prodAndCounts.Where(pac => pac.Key.IsRemoved == false && pac.Key.QuantityInStock > 0)
                                     .Select(pc => new ProductModel(pc.Key));
    }

    private void OnBasketServiceStateChanged()
    {
        OnPropertyChanged(nameof(IsEmpty));
        OnPropertyChanged(nameof(TotalProductsCount));
        OnPropertyChanged(nameof(TotalProductsCostWithDiscount));
        OnPropertyChanged(nameof(TotalDiscountSum));
    }
}
