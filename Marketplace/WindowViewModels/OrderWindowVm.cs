using Marketplace.PageViewModels;

namespace Marketplace.WindowViewModels;

public class OrderWindowVm : WindowVmBase
{
    private BasketPageVm _basketModel;

    public OrderWindowVm(BasketPageVm basketModel)
    {
        _basketModel = basketModel;
    }
}
