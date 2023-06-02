using Marketplace.Database.Models;
using Marketplace.Models;
using Marketplace.Models.Base;

namespace Marketplace.WindowModels.Base;

public abstract class WindowProductModelBase : ProductModelBase
{
    protected WindowProductModelBase(Product product) : base(product)
    {
    }
}
