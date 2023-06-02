using System.Linq;
using CommunityToolkit.Mvvm.Input;
using Marketplace.Database.Models;
using Marketplace.Models.Base;
using Marketplace.Pages;
using Marketplace.WindowModels;
using Marketplace.WindowViewModels;
using Marketplace.WindowViews;

namespace Marketplace.PageModels.Base;

public abstract partial class PageProductModelBase : ProductModelBase
{
    public byte[]? MainPhoto => _product.ProductPhotos.FirstOrDefault()?.Data;

    [RelayCommand]
    private void OpenProductModelWindow() =>
        new TitledContainerWindow(
        new ProductDetailsWindowVm(
            new ProductDetailsWindowProductModel(_product))).ShowDialog();


    [RelayCommand]
    private void NavigateToBasket()
    {
        App.NavigationService.Navigate(typeof(BasketPage));
        App.NavigationWindowVm.CurrentPageTitle = $"Корзина ({App.BasketService.TotalItemsCount})";
    }

    protected PageProductModelBase(Product product) : base(product)
    {
    }
}
