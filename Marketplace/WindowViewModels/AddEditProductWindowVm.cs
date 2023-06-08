using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Windows;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Marketplace.Database;
using Marketplace.Database.Models;
using Marketplace.DataTypes.Enums;
using Marketplace.Models;
using Microsoft.Win32;

namespace Marketplace.WindowViewModels;

public partial class AddEditProductWindowVm : WindowVmBase
{
    #region Properties
    public Product Product { get; init; }

    public IEnumerable<ProductManufacturer> Manufacturers { get; init; }

    public IEnumerable<ProductCategory> Categories { get; init; }

    public IEnumerable<ProductStatus> Statuses { get; init; }
    #endregion

    #region Photo functionality
    private IEnumerable<byte[]> Photos => Product.ProductPhotos.Select(pp => pp.Data);

    public int TotalPhotosNumber => Photos.Count();

    public byte[]? MainPhoto => Photos.FirstOrDefault();

    private int _currentPhotoIndex = 0;
    public byte[]? CurrentPhoto => Photos.Any() ? Photos.ElementAt(_currentPhotoIndex) : null;
    public int CurrentPhotoNumber => _currentPhotoIndex + 1;

    private string _name;
    [Required(ErrorMessage = "Обязательное поле")]
    public string Name
    {
        get => Product.Name;
        set
        {
            ValidateProperty(value);
            Product.Name = value;
            OnPropertyChanged();
            SaveChangesCommand.NotifyCanExecuteChanged();
        }
    }

    private string _description;
    [Required(ErrorMessage = "Обязательное поле")]
    public string Description
    {
        get => Product.Description;
        set
        {
            ValidateProperty(value);
            Product.Description = value;
            OnPropertyChanged();
            SaveChangesCommand.NotifyCanExecuteChanged();
        }
    }

    public ProductCategory Category
    {
        get => Product.Category;
        set
        {
            Product.Category = value;
            OnPropertyChanged();
            SaveChangesCommand.NotifyCanExecuteChanged();
        }
    }

    public ProductManufacturer Manufacturer
    {
        get => Product.Manufacturer;
        set
        {
            Product.Manufacturer = value;
            OnPropertyChanged();
            SaveChangesCommand.NotifyCanExecuteChanged();
        }
    }

    public ProductStatus Status
    {
        get => Product.Status;
        set
        {
            Product.Status = value;
            OnPropertyChanged();
            SaveChangesCommand.NotifyCanExecuteChanged();
        }
    }

    private string _cost;
    [Required(ErrorMessage = "Обязательное поле")]
    [RegularExpression(@"\d+", ErrorMessage = "Ожидается число")]
    public string Cost
    {
        get => _cost;
        set
        {
            ValidateProperty(value);
            _cost = value;
            if (decimal.TryParse(value, out var cost))
                Product.Cost = cost;

            OnPropertyChanged();
            SaveChangesCommand.NotifyCanExecuteChanged();
        }
    }

    public int DiscountPercent
    {
        get => Product.DiscountPercent;
        set
        {
            Product.DiscountPercent = value;
            OnPropertyChanged();
            SaveChangesCommand.NotifyCanExecuteChanged();
        }
    }

    public int QuantityInStock
    {
        get => Product.QuantityInStock;
        set
        {
            Product.QuantityInStock = value;
            OnPropertyChanged();
            SaveChangesCommand.NotifyCanExecuteChanged();
        }
    }

    [RelayCommand(CanExecute = nameof(CanShowPrevPhoto))]
    private void ShowPrevPhoto()
    {
        _currentPhotoIndex -= 1;
        ShowPrevPhotoCommand.NotifyCanExecuteChanged();
        ShowNextPhotoCommand.NotifyCanExecuteChanged();
        OnPropertyChanged(nameof(CurrentPhoto));
        OnPropertyChanged(nameof(CurrentPhotoNumber));
        SaveChangesCommand.NotifyCanExecuteChanged();
    }
    private bool CanShowPrevPhoto() => _currentPhotoIndex > 0;


    [RelayCommand(CanExecute = nameof(CanShowNextPhoto))]
    private void ShowNextPhoto()
    {
        _currentPhotoIndex += 1;
        ShowPrevPhotoCommand.NotifyCanExecuteChanged();
        ShowNextPhotoCommand.NotifyCanExecuteChanged();
        OnPropertyChanged(nameof(CurrentPhoto));
        OnPropertyChanged(nameof(CurrentPhotoNumber));
        SaveChangesCommand.NotifyCanExecuteChanged();
    }
    private bool CanShowNextPhoto() => _currentPhotoIndex < TotalPhotosNumber - 1;

    [RelayCommand]
    private void AddPhoto()
    {
        var openFileDialog = new OpenFileDialog
        {
            Filter = "Изображение (png, jpg, jpeg)|*.png;*.jpg;*.jpeg",
            CheckFileExists = true
        };

        if (openFileDialog.ShowDialog() != true)
            return;

        var photoFilePath = openFileDialog.FileName;
        var photoFileBytes = File.ReadAllBytes(photoFilePath);

        var newProductPhoto = new ProductPhoto
        {
            Data = photoFileBytes
        };
        //DatabaseContext.Entities.ProductPhotos.Local.Add(newProductPhoto);
        Product.ProductPhotos.Add(newProductPhoto);

        _currentPhotoIndex = Photos.Count() - 1;

        OnPropertyChanged(nameof(CurrentPhoto));
        OnPropertyChanged(nameof(CurrentPhotoNumber));
        OnPropertyChanged(nameof(TotalPhotosNumber));
        ShowNextPhotoCommand.NotifyCanExecuteChanged();
        ShowPrevPhotoCommand.NotifyCanExecuteChanged();
        SaveChangesCommand.NotifyCanExecuteChanged();
    }

    [RelayCommand]
    private void DeletePhoto()
    {
        var productPhotoToRemove = Product.ProductPhotos.ElementAt(_currentPhotoIndex);
        Product.ProductPhotos.Remove(productPhotoToRemove);

        _currentPhotoIndex -= 1;

        OnPropertyChanged(nameof(CurrentPhoto));
        OnPropertyChanged(nameof(CurrentPhotoNumber));
        OnPropertyChanged(nameof(TotalPhotosNumber));
        ShowNextPhotoCommand.NotifyCanExecuteChanged();
        ShowPrevPhotoCommand.NotifyCanExecuteChanged();
        SaveChangesCommand.NotifyCanExecuteChanged();
    }
    #endregion

    #region Commands
    [RelayCommand(CanExecute = nameof(CanSaveChanges))]
    private void SaveChanges()
    {
        SaveChangesCommand.NotifyCanExecuteChanged();
        ValidateAllProperties();
        if (HasErrors)
            return;

        if (Product.Id == 0)
            DatabaseContext.Entities.Products.Local.Add(Product);
        DatabaseContext.Entities.SaveChanges();

        CloseWindow();
    }
    private bool CanSaveChanges()
    {
        if (Product.Id == 0)
            return HasErrors == false;
        
        return HasErrors == false && DatabaseContext.Entities.HasChanges();
    }
    #endregion

    public AddEditProductWindowVm(Product? product = null)
    {
        Title = product == null ? "Добавление товара" : "Редактирование товара";

        Product = product ?? new Product
        {
            Salesman = App.UserService.CurrentUser.Salesman!
        };

        _cost = product?.Cost.ToString("0") ?? "";

        Manufacturers = DatabaseContext.Entities.ProductManufacturers
            .ToList();
        Manufacturer = Manufacturers.First();

        Statuses = Enum.GetValues(typeof(ProductStatus)).Cast<ProductStatus>().Skip(1);
        Status = Statuses.First();

        Categories = Enum.GetValues(typeof(ProductCategory)).Cast<ProductCategory>().Skip(1);
        Category = Categories.First();
    }
}
