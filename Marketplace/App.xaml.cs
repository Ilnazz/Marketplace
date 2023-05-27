using System.Windows;
using Marketplace.Database.Models;
using Marketplace.WindowViewModels;
using Marketplace.WindowViews;

namespace Marketplace;

public partial class App : Application
{
    public static User? CurrentUser { get; set; }

    protected override void OnStartup(StartupEventArgs e)
        => new ContainerWindow(new NavigationWindowVm()).Show();
}
