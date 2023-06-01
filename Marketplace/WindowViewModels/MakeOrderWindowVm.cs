using System.Collections.Generic;
using System.Linq;
using Marketplace.Database;
using Marketplace.Database.Models;
using Marketplace.PageViewModels;

namespace Marketplace.WindowViewModels;

public partial class MakeOrderWindowVm : WindowVmBase
{
    public IEnumerable<ProductModel> ProductModels { get; set; }

    public MakeOrderWindowVm()
    {
        Title = "Оформление заказа";

        //ProductModels = App.BasketService.GetItemAndCounts().Select(pac => new Prod);
    }
}
