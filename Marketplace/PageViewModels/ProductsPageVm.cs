using Marketplace.Database;
using Marketplace.Database.Models;
using Marketplace.PageModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Marketplace.PageViewModels;

public partial class ProductsPageVm : PageVmBase
{
    #region Properties
    public IEnumerable<ProductsPageProductModel> ProductModels => GetFilteredAndSortedProducts();

    public bool AreThereProducts => ProductModels.Count() > 0;

    public IEnumerable<Sorting<ProductsPageProductModel>> Sortings { get; init; } = new[]
    {
        new Sorting<ProductsPageProductModel>(DefaultSortingName, null! ),

        new Sorting<ProductsPageProductModel>("Название ↑", pms => pms.OrderBy(pm => pm.Name) ),
        new Sorting<ProductsPageProductModel>("Название ↓", pms => pms.OrderByDescending(pm => pm.Name) ),

        new Sorting<ProductsPageProductModel>("Цена ↑", pms => pms.OrderBy(pm => pm.Cost) ),
        new Sorting<ProductsPageProductModel>("Цена ↓", pms => pms.OrderByDescending(pm => pm.Cost) )
    };

    private Sorting<ProductsPageProductModel> _slectedSorting = null!;
    public Sorting<ProductsPageProductModel> SelectedSorting
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
    #endregion

    #region Fields
    private const string DefaultFilterName = "Любой";
    private const string DefaultSortingName = "По умолчнию";

    private readonly IEnumerable<ProductsPageProductModel> _allProductModels = null!;

    private string _searchText = string.Empty;
    #endregion

    public ProductsPageVm(DataTypes.Enums.ProductCategory category)
    {
        DatabaseContext.Entities.Products.Load();

        _allProductModels = DatabaseContext.Entities.Products.Local
            .Where(p => p.ProductCategoryId == (int)category)
            .Select(p => new ProductsPageProductModel(p));

        Manufacturers = DatabaseContext.Entities.ProductManufacturers
            .ToList()
            .Prepend(new ProductManufacturer { Name = DefaultFilterName });

        SelectedSorting = Sortings.First();
        SelectedManufacturer = Manufacturers.First();

        App.SearchService.SearchTextChanged += newSearchText =>
        {
            _searchText = newSearchText;
            OnPropertyChanged(nameof(ProductModels));
            OnPropertyChanged(nameof(AreThereProducts));
        };
    }

    #region Private methods
    private IEnumerable<ProductsPageProductModel> GetFilteredAndSortedProducts()
    {
        var filtered = _allProductModels.Where(ProductModelFilter);
        var sorted = SortProducts(filtered);
        return sorted;
    }

    private IEnumerable<ProductsPageProductModel> SortProducts(IEnumerable<ProductsPageProductModel> productModels)
        => SelectedSorting.Sorter != null ? SelectedSorting.Sorter(productModels) : productModels;

    private bool ProductModelFilter(ProductsPageProductModel pm)
        => SearchFilter(pm) &&
           ManufacturerFilter(pm);

    private bool SearchFilter(ProductsPageProductModel pm)
        =>  pm.Name.ToLower().Contains(_searchText.Trim().ToLower());

    private bool ManufacturerFilter(ProductsPageProductModel pm)
        => SelectedManufacturer.Name == DefaultFilterName || pm.Manufacturer == SelectedManufacturer;
    #endregion
}

public record Sorting<T>(string Name, Func<IEnumerable<T>, IEnumerable<T>> Sorter);