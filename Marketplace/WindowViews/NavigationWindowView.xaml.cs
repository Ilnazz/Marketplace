using Marketplace.Services;
using System.Windows.Controls;

namespace Marketplace.WindowViews;

public partial class NavigationWindowView : UserControl
{
    public NavigationWindowView()
    {
        InitializeComponent();

        NavigationSideBar.PageService = App.PageService;
        App.NavigationService = NavigationSideBar;
    }
}