using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Marketplace.Database;
using Marketplace.Services;
using Marketplace.WindowViews;
using Microsoft.EntityFrameworkCore;

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
    private void Authorize()
    {
        ValidateAllProperties();
        if (HasErrors)
            return;

        if (App.UserService.TryAuthorizeUser(Login!, Password!))
        {
            CloseWindow();
            return;
        }

        var dialogWindow = new Wpf.Ui.Controls.MessageBox
        {
            Content = new TextBlock
            {
                Text = "Пользователь с такими данными не зарегистрирован.",
                TextWrapping = TextWrapping.Wrap,
                FontSize = 18,
                TextAlignment = TextAlignment.Center,
            },
            Width = 500,
            SizeToContent = SizeToContent.Height,
            ResizeMode = ResizeMode.NoResize,
            Title = "Ошибка"
        };
        var okBtn = new Wpf.Ui.Controls.Button
        {
            Content = "Ок",
            HorizontalAlignment = HorizontalAlignment.Stretch,
        };
        okBtn.Click += (_, _) => dialogWindow.Close();
        dialogWindow.Footer = okBtn;
        dialogWindow.ShowDialog();
    }
    private bool CanAuthorize() => HasErrors == false;

    [RelayCommand]
    private void OpenRegWindow()
    {
        CloseWindow();
        new TitledContainerWindow(new RegWindowVm()).ShowDialog();
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
