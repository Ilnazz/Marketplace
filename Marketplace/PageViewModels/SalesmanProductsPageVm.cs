using CommunityToolkit.Mvvm.Input;
using Marketplace.Database;
using Marketplace.Database.Models;
using Marketplace.DataTypes.Enums;
using Marketplace.DataTypes.Records;
using Marketplace.Models;
using Marketplace.WindowViewModels;
using Marketplace.WindowViews;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Marketplace.PageViewModels;

public partial class SalesmanProductsPageVm : PageVmBase
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

    #region Commands
    [RelayCommand]
    private void AddProduct()
    {
        var vm = new AddEditProductWindowVm(null);
        var view = new AddEditProductWindowView() { DataContext = vm };

        var dialogWindow = new Wpf.Ui.Controls.MessageBox
        {
            Content = view,
            Width = view.Width + 30,
            Height = view.Height,
            SizeToContent = SizeToContent.Height,
            ResizeMode = ResizeMode.NoResize,
            Title = vm.Title,
            Topmost = false,
            ShowFooter = false,
            WindowStartupLocation = WindowStartupLocation.CenterScreen
        };
        vm.CloseWindowMethod = dialogWindow.Close;
        dialogWindow.Closing += (_, e) => e.Cancel = vm.OnClosing() == false;
        dialogWindow.ShowDialog();

        OnPropertyChanged(nameof(ProductModels));
        OnPropertyChanged(nameof(AreThereProducts));
    }

    [RelayCommand]
    private void DeleteProduct(Product product)
    {
        DatabaseContext.Entities.Products.Local.Remove(product);
        DatabaseContext.Entities.SaveChanges();

        OnPropertyChanged(nameof(ProductModels));
        OnPropertyChanged(nameof(AreThereProducts));
    }
    #endregion

    #region Fields
    private const string DefaultFilterName = "Любой";

    private readonly IEnumerable<ProductModel> _allProductModels = null!;

    private string _searchText = string.Empty;
    #endregion

    public SalesmanProductsPageVm()
    {
        DatabaseContext.Entities.Products.Load();

        _allProductModels = DatabaseContext.Entities.Products.Local
            .Where(p => p.Salesman == App.UserService.CurrentUser.Salesman!)
            .Select(p => new ProductModel(p));

        foreach (var pm in _allProductModels)
            pm.PropertyChanged += (_, _) =>
            {
                OnPropertyChanged(nameof(ProductModels));
                OnPropertyChanged(nameof(AreThereProducts));
            };

        Manufacturers = DatabaseContext.Entities.ProductManufacturers
            .ToList()
            .Prepend(new ProductManufacturer { Name = DefaultFilterName });

        SelectedSorting = Sortings.First();
        SelectedManufacturer = Manufacturers.First();

        Statuses = Enum.GetValues(typeof(ProductStatus)).Cast<ProductStatus>();
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
        => pm.Name.ToLower().Contains(_searchText.Trim().ToLower());

    private bool ManufacturerFilter(ProductModel pm)
        => SelectedManufacturer.Name == DefaultFilterName || pm.Manufacturer == SelectedManufacturer;

    private bool StatusFilter(ProductModel pm)
        => SelectedStatus == ProductStatus.None || pm.Product.Status == SelectedStatus;
    #endregion
}