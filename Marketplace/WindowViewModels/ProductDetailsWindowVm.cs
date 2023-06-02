using Marketplace.Models;

namespace Marketplace.WindowViewModels;

public class ProductDetailsWindowVm : WindowVmBase
{
    public ProductModel ProductModel { get; init; }

    public ProductDetailsWindowVm(ProductModel productModel)
    {
        Title = "Информация о товаре";
        ProductModel = productModel;
    }
}