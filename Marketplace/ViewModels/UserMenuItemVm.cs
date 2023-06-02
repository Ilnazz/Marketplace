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
    #region Properties
    public User? User => App.UserService.CurrentUser;

    public bool IsAuthorized => User != null;

    #endregion

    #region Commands
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

    
    [RelayCommand]
    private void ShowProfileWindow() =>
        new TitledContainerWindow(new UserProfileWindowVm(User!)).Show();


    [RelayCommand]
    private void LogOut() =>
        App.UserService.LogOutUser();
    #endregion

    public UserMenuItemVm()
    {
        App.UserService.StateChanged += () =>
        {
            OnPropertyChanged(nameof(User));
            OnPropertyChanged(nameof(IsAuthorized));
        };
    }
}