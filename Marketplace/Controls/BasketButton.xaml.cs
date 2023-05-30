using System.Windows;
using Wpf.Ui.Controls;

namespace Marketplace.Controls;

public partial class BasketButton : Button
{
    public int ItemsCount
    {
        get { return (int)GetValue(ItemsCountProperty); }
        set { SetValue(ItemsCountProperty, value); }
    }

    public static readonly DependencyProperty ItemsCountProperty =
        DependencyProperty.Register("ItemsCount", typeof(int), typeof(BasketButton));

    public bool IsActive
    {
        get { return (bool)GetValue(IsActiveProperty); }
        set { SetValue(IsActiveProperty, value); }
    }

    public static readonly DependencyProperty IsActiveProperty =
        DependencyProperty.Register("IsActive", typeof(bool), typeof(BasketButton));

    public BasketButton()
    {
        InitializeComponent();
        DataContext = this;
    }
}
