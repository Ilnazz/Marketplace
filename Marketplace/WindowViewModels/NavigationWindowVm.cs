using CommunityToolkit.Mvvm.ComponentModel;

namespace Marketplace.WindowViewModels;

public partial class NavigationWindowVm : WindowVmBase
{
    [ObservableProperty]
    private string _currentPageTitle;

    public NavigationWindowVm()
    {
        Title = "Маркетплэйс";
    }
}
