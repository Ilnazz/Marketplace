using Marketplace.Database.Models;

namespace Marketplace.WindowViewModels;

public class ProductWindowVm : WindowVmBase
{
    public Product Product { get; init; }

    public string Name
    {
        get => Product.Name;
        set
        {
            Product.Name = value;

            ValidateProperty(value);
            OnPropertyChanged();
        }
    }

    public string Description
    {
        get => Product.Description;
        set
        {
            Product.Description = value;

            ValidateProperty(value);
            OnPropertyChanged();
        }
    }

    public decimal Cost
    {
        get => Product.Cost;
        set
        {
            Product.Cost = value;

            ValidateProperty(value);
            OnPropertyChanged();
        }
    }

    // Photos

    public ProductWindowVm(Product product)
    {
        Product = product;
    }
}
