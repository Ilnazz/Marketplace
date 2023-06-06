using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.Input;
using Marketplace.Database;
using Marketplace.Database.Models;
using Marketplace.DataTypes.Enums;
using Marketplace.Models;

namespace Marketplace.WindowViewModels;

public partial class AddEditProductWindowVm : WindowVmBase
{
    #region Properties
    public ProductModel ProductModel { get; init; }

    public IEnumerable<ProductManufacturer> Manufacturers { get; init; }

    public IEnumerable<ProductCategory> Categories { get; init; }

    public IEnumerable<ProductStatus> Statuses { get; init; }
    #endregion

    #region Commands
    [RelayCommand(CanExecute = nameof(CanSaveChanges))]
    private void SaveChanges()
    {
        ValidateAllProperties();
        if (HasErrors)
            return;

        DatabaseContext.Entities.SaveChanges();
        CloseWindow();
    }
    private bool CanSaveChanges() => HasErrors == false && DatabaseContext.Entities.HasChanges();
    #endregion

    private Product _product;

    public AddEditProductWindowVm(ProductModel? productModel = null)
    {
        Title = productModel == null ? "Добавление продукта" : "Редактирование продукта";

        if (productModel == null)
        {
            _product = new Product();
            ProductModel = new ProductModel(_product);
        }
        else
            _product = productModel.Product;

        Manufacturers = DatabaseContext.Entities.ProductManufacturers
            .ToList();

        Statuses = Enum.GetValues(typeof(ProductStatus)).Cast<ProductStatus>().Skip(1);

        Categories = Enum.GetValues(typeof(ProductCategory)).Cast<ProductCategory>().Skip(1);
    }
}
