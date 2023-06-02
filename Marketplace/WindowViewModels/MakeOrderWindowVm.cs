using System.Collections.Generic;
using Marketplace.WindowModels;

namespace Marketplace.WindowViewModels;

public partial class MakeOrderWindowVm : WindowVmBase
{
    public IEnumerable<MakeOrderWindowProductModel> ProductModels { get; set; }

    public MakeOrderWindowVm()
    {
        Title = "Оформление заказа";

        //ProductModels = App.BasketService.GetItemAndCounts().Select(pac => new Prod);
    }
}
