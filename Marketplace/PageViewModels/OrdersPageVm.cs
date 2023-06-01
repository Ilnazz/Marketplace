using System.Windows;
using CommunityToolkit.Mvvm.Input;
using Marketplace.Database.Models;
using Marketplace.WindowViewModels;
using Marketplace.WindowViews;

namespace Marketplace.PageViewModels;

public partial class OrdersPageVm : PageVmBase
{
    public bool IsUserAuthorized => App.UserService.IsUserAuthorized();

    public bool AreThereOrders => ((Client)App.UserService.CurrentUser).Orders.Count > 0;

    [RelayCommand]
    private void Authorize()
    {
        var authWindowVm = new AuthWindowVm();
        var authWindowView = new AuthWindowView() { DataContext = authWindowVm };

        var dialogWindow = new Wpf.Ui.Controls.MessageBox
        {
            Content = authWindowView,
            Width = authWindowView.Width + 30,
            Height = authWindowView.Height,
            SizeToContent = SizeToContent.Height,
            ResizeMode = ResizeMode.NoResize,
            Title = authWindowVm.Title,
            ShowFooter = false,
            WindowStartupLocation = WindowStartupLocation.CenterScreen
        };
        authWindowVm.CloseWindowMethod += dialogWindow.Close;
        dialogWindow.ShowDialog();
    }

    public OrdersPageVm()
    {
        App.UserService.StateChanged += () =>
        {
            OnPropertyChanged(nameof(IsUserAuthorized));
            OnPropertyChanged(nameof(AreThereOrders));
        };
    }
}