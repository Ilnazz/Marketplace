using System.Collections.Generic;
using Marketplace.Models;

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
