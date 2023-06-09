using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Marketplace.Pages;

namespace Marketplace.ViewModels;

public partial class DeliveryPointMenuItemVm : ObservableObject
{
    public bool IsEmployee => App.UserService.CurrentUser.Role == DataTypes.Enums.UserRole.Employee;

    #region Commands
    [RelayCommand]
    private void NavigateToCategoriesPage()
    {
        if (App.NavigationWindowVm.CurrentPageTitle?.StartsWith("Пункты выдачи товаров") == false)
        {
            App.NavigationService.Navigate(typeof(DeliveryPointsPage));
            UpdateNavWindowCurrentPageTitle();
        }

        App.SearchService.IsEnabled = false;
    }
    #endregion

    public DeliveryPointMenuItemVm()
    {
        App.UserService.StateChanged += () =>
        {
            OnPropertyChanged(nameof(IsEmployee));
            App.NavigationService.Navigate(typeof(BookProductsPage));
        };
    }

    private void UpdateNavWindowCurrentPageTitle() =>
        App.NavigationWindowVm.CurrentPageTitle = "Пункты выдачи товаров";
}