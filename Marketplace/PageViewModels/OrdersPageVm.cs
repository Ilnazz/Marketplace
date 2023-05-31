using CommunityToolkit.Mvvm.Input;
using Marketplace.WindowViewModels;
using Marketplace.WindowViews;

namespace Marketplace.PageViewModels;

public partial class OrdersPageVm : PageVmBase
{
    public bool IsUserAuthorized => App.UserService.IsUserAuthorized();

    [RelayCommand]
    private void Authorize()
    {
        new TitledContainerWindow(new AuthWindowVm()).ShowDialog();
        OnPropertyChanged(nameof(IsUserAuthorized));
    }
}