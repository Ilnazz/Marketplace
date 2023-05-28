using Marketplace.Database;

namespace Marketplace.WindowViewModels;

public class ProductWindowVm : WindowVmBase
{
    #region Properties
    public ProductModel ProductModel { get; init; }
    #endregion

    public ProductWindowVm(ProductModel product)
    {
        Title = "Информация о товаре";
        ProductModel = product;
    }
}
