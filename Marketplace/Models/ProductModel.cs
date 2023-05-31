using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Marketplace.Database.Models;
using Marketplace.WindowViewModels;
using Marketplace.WindowViews;
using Wpf.Ui.Controls;

namespace Marketplace.Database;

public partial class ProductModel : ObservableObject
{
    #region Main properties
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

    public int DiscountPercent
    {
        get => _product.DiscountPercent;
        set
        {
            _product.DiscountPercent = value;
            OnPropertyChanged();
        }
    }
    public bool HasDiscount => DiscountPercent > 0;
    public decimal CostWithDiscount => Cost - Cost * ((decimal)DiscountPercent / 100);

    public decimal TotalCost => Cost * QuantityInBasket;
    public decimal TotalCostWithDiscount => CostWithDiscount * QuantityInBasket;

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

    public Salesman Salesman
    {
        get => _product.Salesman;
        set
        {
            _product.Salesman = value;
            OnPropertyChanged();
        }
    }

    public int QuantityInStock
    {
        get => _product.QuantityInStock;
        set
        {
            _product.QuantityInStock = value;
            OnPropertyChanged();
        }
    }
    public bool AvailableInStock => QuantityInStock > 0;
    #endregion

    [RelayCommand]
    private void OpenProductModelWindow()
    {
        var productWindowVm = new ProductDetailsWindowVm(this);
        new TitledContainerWindow(productWindowVm).ShowDialog();
    }

    #region Basket
    public int QuantityInBasket => App.BasketService.GetCount(_product);
    public bool IsInBasket => App.BasketService.GetCount(_product) > 0;


    [RelayCommand(CanExecute = nameof(CanPutToBasket))]
    private void PutToBasket(Flyout messageFlyout)
    {
        AddOneToBasket(messageFlyout);
        PutToBasketCommand.NotifyCanExecuteChanged();
    }
    private bool CanPutToBasket() => AvailableInStock && QuantityInBasket == 0;


    [RelayCommand]
    private void AddOneToBasket(Flyout messageFlyout)
    {
        if (QuantityInBasket >= QuantityInStock)
            messageFlyout.Show();
        else
            App.BasketService.AddToBasket(_product, 1);

        RemoveOneFromBasketCommand.NotifyCanExecuteChanged();
        OnPropertyChanged(nameof(QuantityInBasket));
        OnPropertyChanged(nameof(IsInBasket));
    }


    [RelayCommand(CanExecute = nameof(CanRemoveOneFromBasket))]
    private void RemoveOneFromBasket()
    {
        App.BasketService.RemoveFromBasket(_product, 1);
        PutToBasketCommand.NotifyCanExecuteChanged();
        OnPropertyChanged(nameof(QuantityInBasket));
        OnPropertyChanged(nameof(IsInBasket));
    }
    private bool CanRemoveOneFromBasket() => QuantityInBasket > 0;
    #endregion

    #region Photo
    private IEnumerable<byte[]> _photos => _product.ProductPhotos.Select(pp => pp.Data);
    public byte[]? CurrentPhoto => _photos.Count() > 0 ? _photos.ElementAt(_currentPhotoIndex) : null;
    
    private int _currentPhotoIndex = 0;
    public int CurrentPhotoNumber => _currentPhotoIndex + 1;
    public int TotalPhotosNumber => _photos.Count();


    [RelayCommand(CanExecute = nameof(CanShowPrevPhoto))]
    private void ShowPrevPhoto()
    {
        _currentPhotoIndex -= 1;
        ShowPrevPhotoCommand.NotifyCanExecuteChanged();
        ShowNextPhotoCommand.NotifyCanExecuteChanged();
        OnPropertyChanged(nameof(CurrentPhoto));
        OnPropertyChanged(nameof(CurrentPhotoNumber));
    }
    private bool CanShowPrevPhoto() => _currentPhotoIndex > 0;


    [RelayCommand(CanExecute = nameof(CanShowNextPhoto))]
    private void ShowNextPhoto()
    {
        _currentPhotoIndex += 1;
        ShowPrevPhotoCommand.NotifyCanExecuteChanged();
        ShowNextPhotoCommand.NotifyCanExecuteChanged();
        OnPropertyChanged(nameof(CurrentPhotoNumber));
    }
    private bool CanShowNextPhoto() => _currentPhotoIndex < TotalPhotosNumber - 1;
    #endregion

    private Product _product;

    public ProductModel(Product product)
    {
        _product = product;

        App.BasketService.StateChanged += () =>
        {
            OnPropertyChanged(nameof(QuantityInBasket));
            OnPropertyChanged(nameof(IsInBasket));
            PutToBasketCommand.NotifyCanExecuteChanged();
            AddOneToBasketCommand.NotifyCanExecuteChanged();
            RemoveOneFromBasketCommand.NotifyCanExecuteChanged();
        };
    }
}
