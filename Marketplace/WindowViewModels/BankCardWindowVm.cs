using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Marketplace.Database;
using Marketplace.Database.Models;
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
        // Todo: extra validation of number

        return ValidationResult.Success!;
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
    private bool CanSaveChanges() => HasErrors == false;
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
