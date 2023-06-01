using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Marketplace.Database;
using Marketplace.Services;
using Marketplace.WindowViews;
using Microsoft.EntityFrameworkCore;
using Wpf.Ui.Controls;

namespace Marketplace.WindowViewModels;

public partial class AuthWindowVm : WindowVmBase
{
    #region [ Properties ]
    [NotifyCanExecuteChangedFor(nameof(AuthorizeCommand))]
    [Required(ErrorMessage = "Обязательное поле.")]
    [NotifyDataErrorInfo]
    [ObservableProperty]
    public string? _login;

    [NotifyCanExecuteChangedFor(nameof(AuthorizeCommand))]
    [Required(ErrorMessage = "Обязательное поле.")]
    [NotifyDataErrorInfo]
    [ObservableProperty]
    public string? _password;
    #endregion

    #region [ Commands ]
    [RelayCommand(CanExecute = nameof(CanAuthorize))]
    private void Authorize(Flyout errorMessageFlyout)
    {
        ValidateAllProperties();
        if (HasErrors)
            return;

        if (App.UserService.TryAuthorizeUser(Login!, Password!))
        {
            CloseWindow();

            return;
        }

        errorMessageFlyout.Show();
    }
    private bool CanAuthorize() => HasErrors == false;

    [RelayCommand]
    private void ShowRegWindow()
    {
        CloseWindow();

        var regWindowVm = new RegWindowVm();
        var regWindowView = new RegWindowView() { DataContext = regWindowVm };

        var dialogWindow = new Wpf.Ui.Controls.MessageBox
        {
            Content = regWindowView,
            Width = regWindowView.Width + 30,
            Height = regWindowView.Height,
            SizeToContent = SizeToContent.Height,
            ResizeMode = ResizeMode.NoResize,
            Title = regWindowVm.Title,
            ShowFooter = false,
            WindowStartupLocation = WindowStartupLocation.CenterScreen
        };
        regWindowVm.CloseWindowMethod += dialogWindow.Close;
        dialogWindow.ShowDialog();
    }
    #endregion

    public AuthWindowVm()
    {
        Title = "Вход в аккаунт";

        DatabaseContext.Entities.Clients.Load();
        DatabaseContext.Entities.Salesmen.Load();
        DatabaseContext.Entities.Employees.Load();
    }
}
