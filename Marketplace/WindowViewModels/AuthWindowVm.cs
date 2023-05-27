using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Marketplace.Services;
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
    private void Authorize()
    {
        ValidateAllProperties();

        if (AuthRegService.AuthorizeUser(Login!, Password!) == false)
        {
            new MessageBox().Show("Ошибка", "Неверные логин и/или пароль.");
            return;
        }

    }
    private bool CanAuthorize() => HasErrors == false;

    [RelayCommand]
    private void GoToRegWindow()
    {
    }
    #endregion

    public AuthWindowVm()
    {
    }
}
