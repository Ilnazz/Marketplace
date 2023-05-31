using System.Collections.Generic;
using System.Linq;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Marketplace.Database;
using Marketplace.Pages;
using Marketplace.Services;
using Marketplace.WindowViewModels;
using Marketplace.WindowViews;

namespace Marketplace.PageViewModels;

public partial class BasketPageVm : PageVmBase
{
    #region Properties
    [ObservableProperty]
    private IEnumerable<ProductModel> _productModels;

    public bool IsEmpty => _productModels.Any() == false;

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
        if (App.UserService.IsUserAuthorized())
        {
            new TitledContainerWindow(new MakeOrderWindowVm(this)).ShowDialog();
            return;
        }

        var dialogWindow = new Wpf.Ui.Controls.MessageBox
        {
            Content = new NeedToLoginWindowView(),
            SizeToContent = SizeToContent.Height,
            ResizeMode = ResizeMode.NoResize,
            Title = "Информация",
            ButtonLeftName = "Да",
            ButtonLeftAppearance = Wpf.Ui.Common.ControlAppearance.Primary,

            ButtonRightName = "Нет"
        };
        dialogWindow.ButtonLeftClick += (_, _) =>
        {
            dialogWindow.Close();
            new TitledContainerWindow(new AuthWindowVm()).ShowDialog();

            if (App.UserService.IsUserAuthorized())
                new TitledContainerWindow(new MakeOrderWindowVm(this)).ShowDialog();
        };
        dialogWindow.ButtonRightClick += (_, _) => dialogWindow.Close();
        dialogWindow.ShowDialog();
    }

    public BasketPageVm()
    {
        App.BasketService.StateChanged += () =>
        {
            RefreshProductModels();
            OnPropertyChanged(nameof(IsEmpty));
            OnPropertyChanged(nameof(TotalProductsCount));
            OnPropertyChanged(nameof(TotalProductsCostWithDiscount));
            OnPropertyChanged(nameof(TotalDiscountSum));
        };
        RefreshProductModels();
    }

    private void RefreshProductModels()
    {
        var itemAndCounts = App.BasketService.GetItemAndCounts();
        ProductModels = itemAndCounts.Select(pc => new ProductModel(pc.Key));
    }
}
