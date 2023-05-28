using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Marketplace.Database;
using Marketplace.Pages;
using Wpf.Ui.Controls;

namespace Marketplace.PageViewModels;

public partial class BasketPageVm : PageVmBase
{
    [ObservableProperty]
    private IEnumerable<ProductModel> _productModels;

    public bool IsEmpty => _productModels.Any() == false;

    [RelayCommand]
    private void GoToProductsPage() => App.NavigationService.Navigate(typeof(ProductsPage));

    public BasketPageVm()
    {
        App.BasketService.StateChanged += () =>
        {
            RefreshProductModels();
            OnPropertyChanged(nameof(IsEmpty));
        };
        RefreshProductModels();
    }

    private void RefreshProductModels()
    {
        var itemAndCounts = App.BasketService.GetItemAndCounts();
        ProductModels = itemAndCounts.Select(pc => new ProductModel(pc.Key));
    }
}
