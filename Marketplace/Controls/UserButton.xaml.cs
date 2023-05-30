using System.Windows;
using Marketplace.Database.Models;
using Wpf.Ui.Controls;

namespace Marketplace.Controls;

public partial class UserButton : Button
{
    public User User
    {
        get { return (User)GetValue(UserProperty); }
        set { SetValue(UserProperty, value); }
    }

    public static readonly DependencyProperty UserProperty =
        DependencyProperty.Register("User", typeof(User), typeof(UserButton), new PropertyMetadata(new User { Surname = "Хасанов"}));

    public bool IsActive
    {
        get { return (bool)GetValue(IsActiveProperty); }
        set { SetValue(IsActiveProperty, value); }
    }

    public static readonly DependencyProperty IsActiveProperty =
        DependencyProperty.Register("IsActive", typeof(bool), typeof(UserButton));

    public UserButton()
    {
        InitializeComponent();
        DataContext = this;
    }
}
