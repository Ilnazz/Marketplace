using CommunityToolkit.Mvvm.Input;
using Marketplace.Database;
using Marketplace.Database.Models;
using Marketplace.WindowViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Wpf.Ui.Controls;

namespace Marketplace.PageViewModels;

public partial class ProductsPageVm : PageVmBase
{
    #region Properties
    public IEnumerable<ProductModel> ProductModels =>
        GetFilteredAndSortedProducts().Select(p => new ProductModel(p));

    private string _searchingText = string.Empty;
    public string SearchingText
    {
        get => _searchingText;
        set
        {
            _searchingText = value;

            OnPropertyChanged();
            OnPropertyChanged(nameof(ProductModels));
        }
    }

    public IEnumerable<Sorting<Product>> Sortings { get; init; } = new[]
    {
        new Sorting<Product>("По умолчанию", null! ),

        new Sorting<Product>("Название ↑", ps => ps.OrderBy(p => p.Name) ),
        new Sorting<Product>("Название ↓", ps => ps.OrderByDescending(p => p.Name) ),

        new Sorting<Product>("Цена ↑", ps => ps.OrderBy(p => p.Cost) ),
        new Sorting<Product>("Цена ↓", ps => ps.OrderByDescending(p => p.Cost) )
    };

    private Sorting<Product> _slectedSorting;
    public Sorting<Product> SelectedSorting
    {
        get => _slectedSorting;
        set
        {
            _slectedSorting = value;

            OnPropertyChanged();
            OnPropertyChanged(nameof(ProductModels));
        }
    }

    public IEnumerable<ProductCategory> Categories { get; init; }

    private ProductCategory _selectedCategory = null!;
    public ProductCategory SelectedCategory
    {
        get => _selectedCategory;
        set
        {
            _selectedCategory = value;

            OnPropertyChanged();
            OnPropertyChanged(nameof(ProductModels));
        }
    }

    public IEnumerable<ProductManufacturer> Manufacturers { get; init; }

    private ProductManufacturer _selectedManufacturer = null!;
    public ProductManufacturer SelectedManufacturer
    {
        get => _selectedManufacturer;
        set
        {
            _selectedManufacturer = value;

            OnPropertyChanged();
            OnPropertyChanged(nameof(ProductModels));
        }
    }
    
    #endregion

    #region Commands
    [RelayCommand]
    private void OpenProductModelWindow(ProductModel product)
    {
        var productWindowVm = new ProductWindowVm(product);

        var messageBox = new MessageBox
        {
            Content = productWindowVm,

            ButtonLeftName = "Сохранить",
            ButtonRightName = "Отменить",

            Height = 400,
            Width = 400,

            ResizeMode = System.Windows.ResizeMode.CanResize,
            ShowInTaskbar = false
        };
        messageBox.ShowDialog();
    }
    #endregion

    #region Fields
    private const string DefaultFilterName = "Нет";
    private const string DefaultSortingName = "По умолчнию";

    private IEnumerable<Product> _allProducts = null!;
    #endregion

    public ProductsPageVm()
    {
        DatabaseContext.Entities.Products.Load();
        
        _allProducts = DatabaseContext.Entities.Products.Local.ToList();

        Categories = DatabaseContext.Entities.ProductCategories.ToList().Prepend(new ProductCategory { Name = DefaultFilterName });
        SelectedCategory = Categories.First();

        Manufacturers = DatabaseContext.Entities.ProductManufacturers.ToList().Prepend(new ProductManufacturer { Name = DefaultFilterName });
        SelectedManufacturer = Manufacturers.First();

        SelectedSorting = Sortings.First();
    }

    #region Private methods
    private IEnumerable<Product> GetFilteredAndSortedProducts()
    {
        var filtered = _allProducts.Where(ProductFilter);
        var sorted = SortProducts(filtered);
        return sorted;
    }

    private IEnumerable<Product> SortProducts(IEnumerable<Product> products)
        => SelectedSorting.Sorter != null ? SelectedSorting.Sorter(products) : products;

    private bool ProductFilter(Product product)
        => SearchingFilter(product) &&
           CategoryFilter(product) &&
           ManufacturerFilter(product);

    private bool SearchingFilter(Product product)
        =>  product.Name.ToLower().Contains(SearchingText.Trim().ToLower());

    private bool CategoryFilter(Product product)
        => SelectedCategory.Name == DefaultFilterName || product.ProductCategory == SelectedCategory;

    private bool ManufacturerFilter(Product product)
        => SelectedManufacturer.Name == DefaultFilterName || product.ProductManufacturer == SelectedManufacturer;
    #endregion
}

public record Sorting<T>(string Name, Func<IEnumerable<T>, IEnumerable<T>> Sorter);