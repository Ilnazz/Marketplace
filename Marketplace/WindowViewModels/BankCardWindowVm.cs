using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Marketplace.Database.Models;

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
        }
    }

    [NotifyDataErrorInfo]
    [Required(ErrorMessage = "Обязательное поле")]
    [ObservableProperty]
    private int _expirationMonth;

    [NotifyDataErrorInfo]
    [Required(ErrorMessage = "Обязательное поле")]
    [ObservableProperty]
    private int _expirationYear;

    [Required(ErrorMessage = "Обязательное поле")]
    public short ValidationCode
    {
        get => _bankCard.ValidationCode;
        set
        {
            ValidateProperty(value);
            _bankCard.ValidationCode = value;
            OnPropertyChanged();
        }
    }
    #endregion

    #region Validation methods
    public static ValidationResult ValidateNumber(string number, ValidationContext context)
    {
        var instance = context.ObjectInstance as MakeOrderWindowVm;

        if (string.IsNullOrWhiteSpace(number))
            return new("Обязательное поле");
        else if (Regex.IsMatch(number, @"\d+") == false)
            return new("Только цифры");
        // Validate also number

        return ValidationResult.Success!;
    }
    #endregion

    #region Commands
    [RelayCommand(CanExecute = nameof(CanSaveChanges))]
    private void SaveChanges()
    {
        ValidateAllProperties();
        if (HasErrors)
            return;


    }
    private bool CanSaveChanges() => true;
    #endregion

    private BankCard _bankCard = null!;

    public BankCardWindowVm(BankCard bankCard)
    {
        Title = "Банковская карта";

        _bankCard = bankCard;
    }
}
