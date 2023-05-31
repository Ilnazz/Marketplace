using System.Windows;
using Marketplace.Database.Models;
using Wpf.Ui.Controls;

namespace Marketplace.Controls;

public partial class OrdersButton : Button
{
    public bool IsActive
    {
        get { return (bool)GetValue(IsActiveProperty); }
        set { SetValue(IsActiveProperty, value); }
    }

    public static readonly DependencyProperty IsActiveProperty =
        DependencyProperty.Register("IsActive", typeof(bool), typeof(OrdersButton));

    public OrdersButton()
    {
        InitializeComponent();
        DataContext = this;
    }
}
