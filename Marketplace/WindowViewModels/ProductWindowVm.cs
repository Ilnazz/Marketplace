using Marketplace.Database;

namespace Marketplace.WindowViewModels;

public class ProductWindowVm : WindowVmBase
{
    public ProductModel ProductModel { get; init;
    }
    public ProductWindowVm(ProductModel product)
    {
        ProductModel = product;
    }
}
