using Marketplace.Services;
using System.Windows.Controls;

namespace Marketplace.WindowViews;

public partial class NavigationWindowView : UserControl
{
    public NavigationWindowView()
    {
        InitializeComponent();

        DataContext = this;

        NavigationSideBar.PageService = new PageService();

        App.Instance.NavigationService = NavigationSideBar;
    }
}