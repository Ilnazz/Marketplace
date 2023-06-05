using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Marketplace.WindowViewModels;

public partial class PayForOrderWindowVm : WindowVmBase
{
    #region Properties
    [ObservableProperty]
    private bool _wasPaid;

    public decimal Cost { get; init; }
    #endregion

    #region Commands
    [RelayCommand]
    private void Pay()
    {
        System.Threading.Thread.Sleep(1500);
        WasPaid = true;
    }

    [RelayCommand]
    private void Ok()
    {
        CloseWindow();
    }
    #endregion

    public PayForOrderWindowVm(decimal orderCost)
    {
        Title = "Оплата заказа";
        Cost = orderCost;
    }
}
