using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Marketplace.Database;
using Marketplace.Database.Models;
using Marketplace.WindowViews;

namespace Marketplace.WindowViewModels;

public partial class ReturnProductWindowVm : WindowVmBase
{
    public Product Product { get; set; }

    public int Min => 1;

    public int Max => _orderProduct.Quantity - _orderProduct.ReturnedQuantity;

    public int Quantity { get; set; }

    private OrderProduct _orderProduct;

    [ObservableProperty]
    private bool _wasReturned;

    #region Commands
    [RelayCommand]
    private void Return()
    {
        Product.QuantityInStock += Quantity;
        _orderProduct.ReturnedQuantity += Quantity;
        WasReturned = true;

        if (App.UserService.CurrentUser.Client.BankCard != null)
            App.UserService.CurrentUser.Client.BankCard.Balance += _orderProduct.Quantity * _orderProduct.Cost;

        var msg = App.UserService.CurrentUser.Client.BankCard != null ? "Товар возвращён. Средства зачислены на карту." : "Товар возвращён";

        var infoWindowVm = new InfoWindowVm(msg, "Информация");
        var infoWindowView = new InfoWindowView() { DataContext = infoWindowVm };

        var dialogWindow = new Wpf.Ui.Controls.MessageBox
        {
            Content = infoWindowView,
            Width = infoWindowView.Width + 30,
            Height = infoWindowView.Height,
            SizeToContent = SizeToContent.Height,
            ResizeMode = ResizeMode.NoResize,
            Title = infoWindowVm.Title,
            Topmost = false,
            ShowFooter = false,
            WindowStartupLocation = WindowStartupLocation.CenterScreen
        };
        infoWindowVm.CloseWindowMethod += dialogWindow.Close;
        dialogWindow.ShowDialog();

        DatabaseContext.Entities.SaveChanges();
        CloseWindow();
    }

    #endregion

    public ReturnProductWindowVm(OrderProduct orderProduct)
    {
        Title = "Возврат товара";

        _orderProduct = orderProduct;
        Product = orderProduct.Product;
    }
}
