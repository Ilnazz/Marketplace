using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Marketplace.DataTypes.Enums;
using Marketplace.Pages;
using Marketplace.PageViewModels;

namespace Marketplace.ViewModels;

public partial class SalesmanProductsMenuItemVm : ObservableObject
{
    public bool UserHasPermission =>
         App.UserService.CurrentUser.Role == UserRole.Salesman;

    [RelayCommand]
    private void NavigateToSalesmanProductsPage()
    {
        if (App.NavigationWindowVm.CurrentPageTitle?.Equals("Мои продукты") == false)
        {
            App.NavigationService.Navigate(typeof(SalesmanProductsPage));
            UpdateNavWindowCurrentPageTitle();
        }

        App.SearchService.IsEnabled = false;
    }
    private void UpdateNavWindowCurrentPageTitle() =>
        App.NavigationWindowVm.CurrentPageTitle = $"Мои продукты";

    public SalesmanProductsMenuItemVm()
    {
        App.UserService.StateChanged += () => OnPropertyChanged(nameof(UserHasPermission));
    }
}
