using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Marketplace.DataTypes.Enums;
using Marketplace.WindowViews;
using Wpf.Ui.Controls;

namespace Marketplace.WindowViewModels;

public partial class RegWindowVm : WindowVmBase
{
    #region [ Properties ]

    public UserRole Role { get; set; }

    [Required(ErrorMessage = "Обязательное поле.")]
    [MinLength(8, ErrorMessage = "Не менее 8 символов.")]
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
    public string? _login;

    [Required(ErrorMessage = "Обязательное поле.")]
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
    [CustomValidation(typeof(RegWindowVm), nameof(ValidatePassword))]
    public string? _password;

    [Required(ErrorMessage = "Обязательное поле.")]
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
    [CustomValidation(typeof(RegWindowVm), nameof(ValidatePasswordConfirmation))]
    public string? _passwordConfirmation;

    #endregion

    #region [ Commands ]
    [RelayCommand]
    private void SelectRole(UserRole role) =>
        Role = role;

    [RelayCommand(CanExecute = nameof(CanRegister))]
    private void Register(Flyout messageFlyout)
    {
        ValidateAllProperties();

        if (App.UserService.TryRegisterUser(Login!, Password!, Role))
        {
            App.UserService.TryAuthorizeUser(Login!, Password!);
            CloseWindow();
            return;
        }

        messageFlyout.Show();
    }
    private bool CanRegister() => HasErrors == false;

    [RelayCommand]
    private void ShowAuthWindow()
    {
        CloseWindow();

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
    #endregion

    #region [ Specific password validation functions ]

    public static ValidationResult ValidatePassword(string password, ValidationContext context)
    {
        var instance = context.ObjectInstance as RegWindowVm;

        if (password.Length < 8)
            return new("Не менее 8 символов.");
        if (password!.Any(char.IsLower) == false)
            return new("Мин. 1 символ нижнего регистра.");
        else if (password!.Any(char.IsUpper) == false)
            return new("Мин. 1 символ верхнего регистра.");
        else if (password!.Any(char.IsDigit) == false)
            return new("Мин. 1 цифра.");
        else if (password!.Any("~`!@#$%^&*()_+№;:?,.".Contains) == false)
            return new("Мин. 1 символ из ~`!@#$%^&*()_+№;:?,.");

        return ValidationResult.Success!;
    }

    public static ValidationResult ValidatePasswordConfirmation(string passwordConfirmation, ValidationContext context)
    {
        var instance = context.ObjectInstance as RegWindowVm;

        if (passwordConfirmation != instance!.Password)
            return new("Пароли должны совпадать.");

        return ValidationResult.Success!;
    }

    #endregion

    public RegWindowVm()
    {
        Title = "Регистрация";
    }
}
