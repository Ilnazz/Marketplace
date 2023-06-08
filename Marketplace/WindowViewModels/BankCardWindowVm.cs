using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Marketplace.Database;
using Marketplace.Database.Models;
using Marketplace.WindowViews;
using Wpf.Ui.Controls;

namespace Marketplace.WindowViewModels;

public partial class BankCardWindowVm : WindowVmBase
{
    #region Properties
    [Required(ErrorMessage = "Обязательное поле")]
    public string Name
    {
        get => _bankCard.Name;
        set
        {
            ValidateProperty(value);
            _bankCard.Name = value;
            OnPropertyChanged();
            SaveChangesCommand.NotifyCanExecuteChanged();
        }
    }

    [CustomValidation(typeof(BankCardWindowVm), nameof(ValidateNumber))]
    public string Number
    {
        get => _bankCard.Number;
        set
        {
            ValidateProperty(value);
            _bankCard.Number = value;
            OnPropertyChanged();
            SaveChangesCommand.NotifyCanExecuteChanged();
        }
    }

    public decimal Balance
    {
        get => _bankCard.Balance;
        set
        {
            _bankCard.Balance = value;
            OnPropertyChanged();
        }
    }

    private int _expirationMonth;
    public int ExpirationMonth
    {
        get => _expirationMonth;
        set
        {
            _expirationMonth = value;
            OnPropertyChanged();
        }
    }

    private int _expirationYear;
    public int ExpirtaionYear
    {
        get => _expirationYear;
        set
        {
            _expirationYear = value;
            OnPropertyChanged();
        }
    }
    // Taking two right numbers of current date
    public int MinExpirtaionYear => int.Parse($"{DateTime.Now.Year}"[2..]);

    [RegularExpression(@"[1-9]\d{2}", ErrorMessage = "Ожидается число из трёх знаков")]
    public string ValidationCode
    {
        get => $"{_bankCard.ValidationCode}";
        set
        {
            ValidateProperty(value);
            if (short.TryParse(value, out var validationCode))
                _bankCard.ValidationCode = validationCode;

            OnPropertyChanged();
            SaveChangesCommand.NotifyCanExecuteChanged();
        }
    }
    #endregion

    #region Validation methods
    public static ValidationResult ValidateNumber(string number)
    {
        if (string.IsNullOrWhiteSpace(number))
            return new("Обязательное поле");
        else if (Regex.IsMatch(number, @"\d+") == false || number.Length < 13)
            return new("Ожидается положительное число от 13 до 19 знаков");
        else if (Mod10Check(number) == false)
            return new("Неверный номер карты");

        return ValidationResult.Success!;
    }

    private static bool Mod10Check(string creditCardNumber)
    {
        //// check whether input string is null or empty
        if (string.IsNullOrEmpty(creditCardNumber))
        {
            return false;
        }

        //// 1.	Starting with the check digit double the value of every other digit 
        //// 2.	If doubling of a number results in a two digits number, add up
        ///   the digits to get a single digit number. This will results in eight single digit numbers                    
        //// 3. Get the sum of the digits
        int sumOfDigits = creditCardNumber.Where((e) => e >= '0' && e <= '9')
                        .Reverse()
                        .Select((e, i) => ((int)e - 48) * (i % 2 == 0 ? 1 : 2))
                        .Sum((e) => e / 10 + e % 10);


        //// If the final sum is divisible by 10, then the credit card number
        //   is valid. If it is not divisible by 10, the number is invalid.            
        return sumOfDigits % 10 == 0;
    }
    #endregion

    #region Commands
    [RelayCommand(CanExecute = nameof(CanSaveChanges))]
    private void SaveChanges()
    {
        ValidateAllProperties();
        SaveChangesCommand.NotifyCanExecuteChanged();

        if (HasErrors)
            return;

        var year = int.Parse($"{DateTime.Now.Year}"[..1] + $"{_expirationYear}");
        _bankCard.ExpirationDate = new DateTime(year, _expirationMonth, 1);

        if (_bankCard.Id == 0)
            DatabaseContext.Entities.BankCards.Local.Add(_bankCard);

        CloseWindow();
    }
    private bool CanSaveChanges() => _bankCard.Id == 0 ? HasErrors == false : HasErrors == false && DatabaseContext.Entities.HasChanges();

    [RelayCommand]
    private void TopUpBalance()
    {
        var vm = new TopUpBalanceWindowVm();
        var view = new TopUpBalanceWindowView() { DataContext = vm };

        var dialogWindow = new Wpf.Ui.Controls.MessageBox
        {
            Content = view,
            Width = view.Width + 30,
            Height = view.Height,
            SizeToContent = SizeToContent.Height,
            ResizeMode = ResizeMode.NoResize,
            Title = vm.Title,
            Topmost = false,
            ShowFooter = false,
            WindowStartupLocation = WindowStartupLocation.CenterScreen
        };
        vm.CloseWindowMethod += dialogWindow.Close;
        dialogWindow.ShowDialog();

        if (vm.Success)
        {
            Balance += vm.Sum;
            DatabaseContext.Entities.SaveChanges();

            var infoWindowVm = new InfoWindowVm("Баланс пополнен", "Информация");
            var infoWindowView = new InfoWindowView() { DataContext = infoWindowVm };

            var dw = new Wpf.Ui.Controls.MessageBox
            {
                Content = infoWindowView,
                Width = infoWindowView.Width + 30,
                Height = infoWindowView.Height,
                SizeToContent = SizeToContent.Height,
                ResizeMode = ResizeMode.NoResize,
                Title = infoWindowVm.Title,
                Topmost = false,
                ShowFooter = false,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            infoWindowVm.CloseWindowMethod += dw.Close;
            dw.ShowDialog();
        }
    }
    #endregion

    private BankCard _bankCard = null!;

    public BankCardWindowVm(BankCard bankCard)
    {
        Title = "Банковская карта";

        _bankCard = bankCard;
        if (string.IsNullOrWhiteSpace(_bankCard.Name))
            _bankCard.Name = "Основная";
    }

    public override bool OnClosing()
    {
        if (DatabaseContext.Entities.HasChanges())
            DatabaseContext.Entities.CancelChanges();
        return true;
    }
}
