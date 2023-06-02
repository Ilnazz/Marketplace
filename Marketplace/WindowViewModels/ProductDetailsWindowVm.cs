using Marketplace.Database;
using Marketplace.WindowModels;

namespace Marketplace.WindowViewModels;

public class ProductDetailsWindowVm : WindowVmBase
{
    public ProductDetailsWindowProductModel ProductModel { get; init; }

    public ProductDetailsWindowVm(ProductDetailsWindowProductModel productModel)
    {
        Title = "Информация о товаре";
        ProductModel = productModel;
    }
}