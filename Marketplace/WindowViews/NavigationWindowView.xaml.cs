using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using Marketplace.Services;

namespace Marketplace.WindowViews;

[ObservableObject]
public partial class NavigationWindowView : UserControl
{
    public NavigationWindowView()
    {
        InitializeComponent();

        App.SearchService = new SearchService(SearchBox);

        App.NavigationService = NavigationSideBar;
        App.NavigationService.PageService = new PageService();
        App.NavigationService.Navigated += (_, _) =>
        {
            App.NavigationWindowVm.CurrentPageTitle = $"{NavigationSideBar.Current?.Content}";
            App.SearchService.IsEnabled = true;
        };
    }
}