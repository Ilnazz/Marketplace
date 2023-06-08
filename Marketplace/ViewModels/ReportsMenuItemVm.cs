using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Marketplace.Pages;

namespace Marketplace.ViewModels;

public partial class ReportsMenuItemVm : ObservableObject
{
    public bool IsEmployee => App.UserService.CurrentUser.Role == DataTypes.Enums.UserRole.Employee;

    #region Commands
    [RelayCommand]
    private void NavigateToReportsPage()
    {
        if (App.NavigationWindowVm.CurrentPageTitle?.StartsWith("Отчёты") == false)
        {
            App.NavigationService.Navigate(typeof(ReportsPage));
            UpdateNavWindowCurrentPageTitle();
        }

        App.SearchService.IsEnabled = false;
    }
    #endregion

    public ReportsMenuItemVm()
    {
        App.UserService.StateChanged += () =>
        {
            OnPropertyChanged(nameof(IsEmployee));
            App.NavigationService.Navigate(typeof(BookProductsPage));
        };
    }

    private void UpdateNavWindowCurrentPageTitle() =>
        App.NavigationWindowVm.CurrentPageTitle = "Отчёты";
}
