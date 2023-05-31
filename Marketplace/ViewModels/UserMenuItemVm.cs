using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Marketplace.Database.Models;
using Marketplace.WindowViewModels;
using Marketplace.WindowViews;
using Wpf.Ui.Controls;

namespace Marketplace.ViewModels;

public partial class UserMenuItemVm : ObservableObject
{
    public User? User => App.UserService.CurrentUser;

    public bool IsAuthorized => User != null;

    [RelayCommand]
    private void ShowMenu(Flyout menuFlyout)
    {
        menuFlyout.Show();
    }

    [RelayCommand]
    private void Authorize()
    {
        var dialogWindow = new Wpf.Ui.Controls.MessageBox
        {
            Content = new NeedToLoginWindowView(),
            SizeToContent = SizeToContent.Height,
            ResizeMode = ResizeMode.NoResize,
            Title = "Информация",
            ButtonLeftName = "Да",
            ButtonLeftAppearance = Wpf.Ui.Common.ControlAppearance.Primary,

            ButtonRightName = "Нет"
        };
        dialogWindow.ButtonLeftClick += (_, _) =>
        {
            dialogWindow.Close();
            new TitledContainerWindow(new AuthWindowVm()).ShowDialog();
        };
        dialogWindow.ButtonRightClick += (_, _) => dialogWindow.Close();
        dialogWindow.ShowDialog();

        if (App.UserService.IsUserAuthorized())
        {
            OnPropertyChanged(nameof(User));
            OnPropertyChanged(nameof(IsAuthorized));
            ShowProfile();
        }
    }

    [RelayCommand]
    private void LogOut()
    {

    }

    [RelayCommand]
    private void ShowProfile()
    {
        new TitledContainerWindow(new UserWindowVm(User!)).Show();
    }

    public UserMenuItemVm()
    {

    }
}