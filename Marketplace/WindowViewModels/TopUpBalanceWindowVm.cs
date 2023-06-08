using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.Input;

namespace Marketplace.WindowViewModels;

public partial class TopUpBalanceWindowVm : WindowVmBase
{
    public bool Success;

    public decimal Sum => decimal.Parse(_topUpSum);

    private string _topUpSum;
    [RegularExpression(@"\d+", ErrorMessage = "Введите сумму пополнения")]
    public string TopUpSum
    {
        get => _topUpSum;
        set
        {
            ValidateProperty(value);
            _topUpSum = value;
            OnPropertyChanged();
        }
    }

    [RelayCommand]
    private void TopUpBalance()
    {
        ValidateAllProperties();
        if (HasErrors)
            return;

        Success = true;
        CloseWindow();
    }
}
