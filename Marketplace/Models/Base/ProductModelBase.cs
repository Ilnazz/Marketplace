using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Marketplace.Database.Models;
using Marketplace.Pages;
using Wpf.Ui.Controls;

namespace Marketplace.Models.Base;

public abstract partial class ProductModelBase : ObservableValidator
{
    #region Main properties

    [Required]
    public string Name
    {
        get => _product.Name;
        set
        {
            ValidateProperty(value);
            _product.Name = value;
            OnPropertyChanged();
        }
    }

    [Required]
    public decimal Cost
    {
        get => _product.Cost;
        set
        {
            ValidateProperty(value);
            _product.Cost = value;
            OnPropertyChanged();
        }
    }

    [Required]
    public int DiscountPercent
    {
        get => _product.DiscountPercent;
        set
        {
            ValidateProperty(value);
            _product.DiscountPercent = value;
            OnPropertyChanged();
        }
    }

    [Required]
    public string Description
    {
        get => _product.Description;
        set
        {
            ValidateProperty(value);
            _product.Description = value;
            OnPropertyChanged();
        }
    }

    [Required]
    public ProductCategory Category
    {
        get => _product.ProductCategory;
        set
        {
            ValidateProperty(value);
            _product.ProductCategory = value;
            OnPropertyChanged();
        }
    }


    [Required]
    public ProductManufacturer Manufacturer
    {
        get => _product.ProductManufacturer;
        set
        {
            ValidateProperty(value);
            _product.ProductManufacturer = value;
            OnPropertyChanged();
        }
    }


    [Required]
    public Salesman Salesman
    {
        get => _product.Salesman;
        set
        {
            ValidateProperty(value);
            _product.Salesman = value;
            OnPropertyChanged();
        }
    }


    [Required]
    public int QuantityInStock
    {
        get => _product.QuantityInStock;
        set
        {
            ValidateProperty(value);
            _product.QuantityInStock = value;
            OnPropertyChanged();
        }
    }


    [Required]
    public bool IsRemoved
    {
        get => _product.IsRemoved;
        set
        {
            ValidateProperty(value);
            _product.IsRemoved = value;
            OnPropertyChanged();
        }
    }
    #endregion

    #region Extra properties
    public bool HasDiscount => DiscountPercent > 0;
    public decimal CostWithDiscount => Cost - Cost * ((decimal)DiscountPercent / 100);

    public bool AvailableInStock => QuantityInStock > 0;

    public int QuantityInBasket => App.BasketService.GetCount(_product);
    public bool IsInBasket => QuantityInBasket > 0;
    #endregion

    #region Commands
    [RelayCommand(CanExecute = nameof(CanPutToBasket))]
    private void PutToBasket(Flyout messageFlyout)
    {
        AddOneToBasket(messageFlyout);
        PutToBasketCommand.NotifyCanExecuteChanged();
    }
    private bool CanPutToBasket() =>
        AvailableInStock && IsRemoved == false && IsInBasket == false;


    [RelayCommand(CanExecute = nameof(CanAddOneToBasket))]
    private void AddOneToBasket(Flyout messageFlyout)
    {
        if (QuantityInBasket >= QuantityInStock)
            messageFlyout.Show();
        else
            App.BasketService.AddToBasket(_product, 1);

        OnPropertyChanged(nameof(IsInBasket));
        OnPropertyChanged(nameof(QuantityInBasket));
        RemoveOneFromBasketCommand.NotifyCanExecuteChanged();
    }
    private bool CanAddOneToBasket() => AvailableInStock || IsRemoved == false;


    [RelayCommand(CanExecute = nameof(CanRemoveOneFromBasket))]
    private void RemoveOneFromBasket()
    {
        App.BasketService.RemoveFromBasket(_product, 1);

        OnPropertyChanged(nameof(IsInBasket));
        OnPropertyChanged(nameof(QuantityInBasket));
        PutToBasketCommand.NotifyCanExecuteChanged();
    }
    private bool CanRemoveOneFromBasket() => QuantityInBasket > 0 && AvailableInStock && (IsRemoved == false);
    #endregion

    protected Product _product;

    public ProductModelBase(Product product) =>
        _product = product;
}
