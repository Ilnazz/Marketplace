using CommunityToolkit.Mvvm.Input;
using Marketplace.Database.Models;
using Marketplace.PageModels.Base;

namespace Marketplace.PageModels;

public partial class BasketPageProductModel : PageProductModelBase
{
    public decimal TotalCost => Cost * QuantityInBasket;
    public decimal TotalCostWithDiscount => CostWithDiscount * QuantityInBasket;


    [RelayCommand]
    private void RemoveFromBasket() =>
        App.BasketService.RemoveFromBasket(_product, QuantityInBasket);

    public BasketPageProductModel(Product product) : base(product)
    {
    }
}
