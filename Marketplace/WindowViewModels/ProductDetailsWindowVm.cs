using Marketplace.Database;

namespace Marketplace.WindowViewModels;

public class ProductDetailsWindowVm : WindowVmBase
{
    public ProductModel ProductModel { get; init; }

    public ProductDetailsWindowVm(ProductModel product)
    {
        Title = "Информация о товаре";
        ProductModel = product;
    }
}