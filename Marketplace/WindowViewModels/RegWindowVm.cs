using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Marketplace.WindowViewModels;

public partial class RegWindowVm : WindowVmBase
{
    #region [ Properties ]

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

    [RelayCommand(CanExecute = nameof(CanRegister))]
    private void Register()
    {
        ValidateAllProperties();

        if (App.AuthRegService.TryRegisterUser(Login!, Password!) == false)
        {
            var dialogWindow = new Wpf.Ui.Controls.MessageBox
            {
                Content = new System.Windows.Controls.TextBlock
                {
                    Text = "Пользователь с такими данными уже зарегистрирован.",
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
            return;
        }

        App.AuthRegService.TryAuthorizeUser(Login!, Password!);
        CloseWindow();
    }
    private bool CanRegister() => HasErrors == false;

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
}
