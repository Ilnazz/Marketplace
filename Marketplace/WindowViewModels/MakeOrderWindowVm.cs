using Marketplace.PageViewModels;

namespace Marketplace.WindowViewModels;

public class MakeOrderWindowVm : WindowVmBase
{
    private BasketPageVm _basketModel;

    public MakeOrderWindowVm(BasketPageVm basketModel)
    {
        _basketModel = basketModel;
    }
}
