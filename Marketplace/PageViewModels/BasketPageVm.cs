using System.Collections.Generic;
using System.Linq;
using Marketplace.Database;

namespace Marketplace.PageViewModels;

public class BasketPageVm : PageVmBase
{
    public IEnumerable<ProductModel> ProductModels { get; }

    public BasketPageVm()
    {
        var itemAndCounts = App.BasketService.GetItemAndCounts();
        ProductModels = itemAndCounts.Select(pc => new ProductModel(pc.Key));
    }
}
