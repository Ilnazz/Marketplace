using Marketplace.Database;
using Marketplace.Database.Models;
using Marketplace.DataTypes.Enums;
using Marketplace.DataTypes.Records;
using Marketplace.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Marketplace.PageViewModels;

public partial class ProductsPageVm : PageVmBase
{
    #region Properties
    public IEnumerable<ProductModel> ProductModels => GetFilteredAndSortedProducts();
    public bool AreThereProducts => ProductModels.Any();

    public IEnumerable<Sorting<ProductModel>> Sortings { get; init; } = new[]
    {
        new Sorting<ProductModel>("По умолчанию", null! ),

        new Sorting<ProductModel>("Название ↑", pms => pms.OrderBy(pm => pm.Name) ),
        new Sorting<ProductModel>("Название ↓", pms => pms.OrderByDescending(pm => pm.Name) ),

        new Sorting<ProductModel>("Цена ↑", pms => pms.OrderBy(pm => pm.CostWithDiscount) ),
        new Sorting<ProductModel>("Цена ↓", pms => pms.OrderByDescending(pm => pm.Cost) )
    };

    private Sorting<ProductModel> _slectedSorting = null!;
    public Sorting<ProductModel> SelectedSorting
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

    public IEnumerable<ProductStatus> Statuses { get; init; }

    private ProductStatus _selectedStatus;
    public ProductStatus SelectedStatus
    {
        get => _selectedStatus;
        set
        {
            _selectedStatus = value;

            OnPropertyChanged();
            OnPropertyChanged(nameof(ProductModels));
            OnPropertyChanged(nameof(AreThereProducts));
        }
    }
    #endregion

    #region Fields
    private const string DefaultFilterName = "Любой";

    private readonly IEnumerable<ProductModel> _allProductModels = null!;

    private string _searchText = string.Empty;
    #endregion

    public ProductsPageVm(ProductCategory productCategory)
    {
        DatabaseContext.Entities.Products.Load();

        _allProductModels = DatabaseContext.Entities.Products.Local
            .Where(p => p.Category == productCategory)
            .Select(p => new ProductModel(p));

        Manufacturers = DatabaseContext.Entities.ProductManufacturers
            .ToList()
            .Prepend(new ProductManufacturer { Name = DefaultFilterName });

        SelectedSorting = Sortings.First();
        SelectedManufacturer = Manufacturers.First();

        Statuses = new ProductStatus[] {ProductStatus.None, ProductStatus.Active, ProductStatus.RemovedFromSale};
        SelectedStatus = Statuses.First();

        App.SearchService.SearchTextChanged += newSearchText =>
        {
            _searchText = newSearchText;
            OnPropertyChanged(nameof(ProductModels));
            OnPropertyChanged(nameof(AreThereProducts));
        };

        App.UserService.StateChanged += () => OnPropertyChanged(nameof(ProductModels));
    }

    #region Private methods
    private IEnumerable<ProductModel> GetFilteredAndSortedProducts()
    {
        var filtered = _allProductModels.Where(ProductModelFilter);
        var sorted = SortProducts(filtered);
        return sorted;
    }

    private IEnumerable<ProductModel> SortProducts(IEnumerable<ProductModel> productModels)
        => SelectedSorting.Sorter != null ? SelectedSorting.Sorter(productModels) : productModels;

    private bool ProductModelFilter(ProductModel pm)
        => SearchFilter(pm) &&
           StatusFilter(pm) &&
           ManufacturerFilter(pm);

    private bool SearchFilter(ProductModel pm)
        =>  pm.Name.ToLower().Contains(_searchText.Trim().ToLower());

    private bool ManufacturerFilter(ProductModel pm)
        => SelectedManufacturer.Name == DefaultFilterName || pm.Manufacturer == SelectedManufacturer;

    private bool StatusFilter(ProductModel pm)
        => SelectedStatus == ProductStatus.None || pm.Product.Status == SelectedStatus;
    #endregion
}