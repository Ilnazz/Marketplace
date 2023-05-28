using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Marketplace.Database.Models;

namespace Marketplace.Database;

public partial class ProductModel : ObservableObject
{
    #region Properties
    public string Name
    {
        get => _product.Name;
        set
        {
            _product.Name = value;
            OnPropertyChanged();
        }
    }

    public decimal Cost
    {
        get => _product.Cost;
        set
        {
            _product.Cost = value;
            OnPropertyChanged();
        }
    }

    public string Description
    {
        get => _product.Description;
        set
        {
            _product.Description = value;
            OnPropertyChanged();
        }
    }

    public ProductCategory Category
    {
        get => _product.ProductCategory;
        set
        {
            _product.ProductCategory = value;
            OnPropertyChanged();
        }
    }

    public ProductManufacturer Manufacturer
    {
        get => _product.ProductManufacturer;
        set
        {
            _product.ProductManufacturer = value;
            OnPropertyChanged();
        }
    }

    [NotifyCanExecuteChangedFor(nameof(PutToBasketCommand))]
    [NotifyCanExecuteChangedFor(nameof(AddOneToBasketCommand))]
    [NotifyCanExecuteChangedFor(nameof(RemoveOneFromBasketCommand))]
    [NotifyPropertyChangedFor(nameof(IsInBasket))]
    [ObservableProperty]
    private int _quantityInBasket;

    public bool IsInBasket => QuantityInBasket > 0;
    #endregion

    #region Commands
    [RelayCommand(CanExecute = nameof(CanPutToBasket))]
    private void PutToBasket() => AddOneToBasket();
    private bool CanPutToBasket() => QuantityInBasket == 0;

    [RelayCommand]
    private void AddOneToBasket()
    {
        QuantityInBasket += 1;
        App.BasketService.AddToBasket(_product, 1);
    }

    [RelayCommand(CanExecute = nameof(CanRemoveOneFromBasket))]
    private void RemoveOneFromBasket()
    {
        QuantityInBasket -= 1;
        App.BasketService.RemoveFromBasket(_product, 1);
    }
    private bool CanRemoveOneFromBasket() => QuantityInBasket > 0;
    #endregion

    private Product _product;

    public ProductModel(Product product)
    {
        _product = product;
    }
}
