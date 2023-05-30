using Marketplace.Database;
using Marketplace.Database.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Marketplace.PageViewModels;

public partial class ProductsPageVm : PageVmBase
{
    #region Properties
    public IEnumerable<ProductModel> ProductModels =>
        GetFilteredAndSortedProducts().Select(p => new ProductModel(p));

    public IEnumerable<Sorting<Product>> Sortings { get; init; } = new[]
    {
        new Sorting<Product>(DefaultSortingName, null! ),

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
            OnPropertyChanged(nameof(AreThereProducts));
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
            OnPropertyChanged(nameof(AreThereProducts));
        }
    }

    public bool AreThereProducts => ProductModels.Count() > 0;
    #endregion

    #region Fields
    private const string DefaultFilterName = "Нет";
    private const string DefaultSortingName = "По умолчнию";

    private IEnumerable<Product> _allProducts = null!;

    private string _searchText = string.Empty;
    #endregion

    public ProductsPageVm(DataTypes.Enums.ProductCategory category)
    {
        DatabaseContext.Entities.Products.Load();

        _allProducts = DatabaseContext.Entities.Products.Local.Where(p => p.ProductCategoryId == (int)category);

        Manufacturers = DatabaseContext.Entities.ProductManufacturers
            .ToList()
            .Prepend(new ProductManufacturer { Name = DefaultFilterName });

        SelectedManufacturer = Manufacturers.First();
        SelectedSorting = Sortings.First();

        App.SearchService.SearchTextChanged += newSearchText =>
        {
            _searchText = newSearchText;
            OnPropertyChanged(nameof(ProductModels));
            OnPropertyChanged(nameof(AreThereProducts));
        };
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
        => SearchFilter(product) &&
           ManufacturerFilter(product);

    private bool SearchFilter(Product product)
        =>  product.Name.ToLower().Contains(_searchText.Trim().ToLower());

    private bool ManufacturerFilter(Product product)
        => SelectedManufacturer.Name == DefaultFilterName || product.ProductManufacturer == SelectedManufacturer;
    #endregion
}

public record Sorting<T>(string Name, Func<IEnumerable<T>, IEnumerable<T>> Sorter);