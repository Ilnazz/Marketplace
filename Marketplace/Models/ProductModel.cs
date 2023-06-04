using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Marketplace.Database;
using Marketplace.Database.Models;
using Marketplace.DataTypes.Enums;
using Marketplace.Pages;
using Marketplace.WindowViewModels;
using Marketplace.WindowViews;
using Wpf.Ui.Controls;

namespace Marketplace.Models;

public partial class ProductModel : ObservableValidator
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
        get => _product.Category;
        set
        {
            ValidateProperty(value);
            _product.Category = value;
            OnPropertyChanged();
        }
    }

    [Required]
    public ProductManufacturer Manufacturer
    {
        get => _product.Manufacturer;
        set
        {
            ValidateProperty(value);
            _product.Manufacturer = value;
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
    public ProductStatus Staus
    {
        get => _product.Status;
        set
        {
            ValidateProperty(value);
            _product.Status = value;
            OnPropertyChanged();
        }
    }

    public bool IsRemoved => Staus == ProductStatus.RemovedFromSale;
    #endregion

    #region Extra properties
    public int AvailableQuantity => _product.AvailableQuantity;

    public bool IsAvailable => _product.IsAvailable;

    public bool HasDiscount => _product.HasDiscount;

    public decimal CostWithDiscount => _product.CostWithDiscount;
    #endregion

    #region Commands
    [RelayCommand]
    private void ShowProductDetailsWindow() =>
        new TitledContainerWindow(
        new ProductDetailsWindowVm(this)).ShowDialog();


    [RelayCommand]
    private void NavigateToBasket()
    {
        App.NavigationService.Navigate(typeof(BasketPage));
        App.NavigationWindowVm.CurrentPageTitle = $"Корзина ({App.BasketService.TotalItemsCount})";
    }
    #endregion

    #region Photo functionality
    private IEnumerable<byte[]> _photos => _product.ProductPhotos.Select(pp => pp.Data);
    public int TotalPhotosNumber => _photos.Count();

    public byte[]? MainPhoto => _photos.FirstOrDefault();

    private int _currentPhotoIndex = 0;
    public byte[]? CurrentPhoto => _photos.Count() > 0 ? _photos.ElementAt(_currentPhotoIndex) : null;
    public int CurrentPhotoNumber => _currentPhotoIndex + 1;


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

    #region Basket functionality
    public int QuantityInBasket => App.BasketService.GetCount(_product);
    public bool IsInBasket => QuantityInBasket > 0;
    public decimal TotalCost => Cost * QuantityInBasket;
    public decimal TotalCostWithDiscount => CostWithDiscount * QuantityInBasket;


    [RelayCommand(CanExecute = nameof(CanPutToBasket))]
    private void PutToBasket()
    {
        AddOneToBasket(null!);
        PutToBasketCommand.NotifyCanExecuteChanged();
    }
    private bool CanPutToBasket() =>
        IsAvailable && Staus == ProductStatus.Active && IsInBasket == false;


    [RelayCommand]
    private void AddOneToBasket(Flyout errorMessageFlyout)
    {
        if (QuantityInBasket + 1 > AvailableQuantity)
        {
            errorMessageFlyout.Show();
            return;
        }

        App.BasketService.AddToBasket(_product, 1);
        OnPropertyChanged(nameof(IsInBasket));
        OnPropertyChanged(nameof(QuantityInBasket));
    }


    [RelayCommand]
    private void RemoveFromBasket() =>
        App.BasketService.RemoveFromBasket(_product, QuantityInBasket);


    [RelayCommand]
    private void RemoveOneFromBasket()
    {
        App.BasketService.RemoveFromBasket(_product, 1);

        OnPropertyChanged(nameof(IsInBasket));
        OnPropertyChanged(nameof(QuantityInBasket));
        PutToBasketCommand.NotifyCanExecuteChanged();
    }
    #endregion

    protected Product _product;

    public ProductModel(Product product)
    {
        _product = product;

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
    }

    private void OnBasketServiceStateChanged()
    {
        OnPropertyChanged(nameof(QuantityInBasket));
        OnPropertyChanged(nameof(IsInBasket));
        OnPropertyChanged(nameof(AvailableQuantity));
        OnPropertyChanged(nameof(IsAvailable));
        PutToBasketCommand.NotifyCanExecuteChanged();
    }
}
