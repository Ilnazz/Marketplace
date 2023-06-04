using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using CommunityToolkit.Mvvm.Input;
using Marketplace.Database;
using Marketplace.Database.Models;
using Marketplace.DataTypes.Enums;
using Marketplace.Models;

namespace Marketplace.WindowViewModels;

public partial class MakeOrderWindowVm : WindowVmBase
{
    #region Properties
    public DateTime DeliveryDate
    {
        get => _order.DeliveryDate;
        set
        {
            _order.DeliveryDate = value;
            OnPropertyChanged();
        }
    }

    [CustomValidation(typeof(MakeOrderWindowVm), nameof(ValidateHomeAddress))]
    public string? HomeAddress
    {
        get => _order.Address;
        set
        {
            _order.Address = value;
            OnPropertyChanged();
        }
    }

    public IEnumerable<DeliveryType> DeliveryTypes { get; init; }
    public DeliveryType DeliveryType
    {
        get => _order.DeliveryType;
        set
        {
            _order.DeliveryType = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(CanSetHomeAddress));
            OnPropertyChanged(nameof(CanSelectDeliveryPoint));
        }
    }

    public IEnumerable<DeliveryPoint> DeliveryPoints { get; init; }
    public DeliveryPoint? DeliveryPoint
    {
        get => _order.DeliveryPoint;
        set
        {
            _order.DeliveryPoint = value;
            OnPropertyChanged();
        }
    }

    public IEnumerable<PaymentMethod> PaymentMethods { get; init; }
    public PaymentMethod PaymentMethod
    {
        get => _order.PaymentMethod;
        set
        {
            _order.PaymentMethod = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(CanAttachBankCard));
        }
    }

    public bool CanSetHomeAddress =>
        DeliveryType == DeliveryType.ToHome;

    public bool CanSelectDeliveryPoint =>
        DeliveryType == DeliveryType.ToDeliveryPoint;

    public bool CanAttachBankCard =>
        PaymentMethod == PaymentMethod.ByBankCard;

    public IEnumerable<ProductModel> ProductModels { get; private set; }
    public int ProductsQuantity => ProductModels.Count();
    #endregion

    #region Validation methods
    public static ValidationResult ValidateHomeAddress(string? address, ValidationContext context)
    {
        var instance = context.ObjectInstance as MakeOrderWindowVm;
        if (instance!.CanSetHomeAddress && string.IsNullOrWhiteSpace(address))
            return new("Укажите адрес");

        return ValidationResult.Success!;
    }
    #endregion

    #region Commands
    [RelayCommand]
    private void MakeOrder()
    {
        ValidateAllProperties();
        if (HasErrors)
            return;

        // Don't forget to validate existence/haveness/attachedness of bank card when payment method is bank_card
    }
    #endregion

    private readonly Order _order = null!;
    public MakeOrderWindowVm(IEnumerable<ProductModel> productModels)
    {
        Title = "Оформление заказа";
        _order = new Order
        {
            Client = App.UserService.CurrentUser.Client!,
            Status = OrderStatus.Created,
            DateTime = DateTime.Now,
            DeliveryDate = DateTime.Now.AddDays(1),
        };

        ProductModels = productModels;

        DeliveryPoints = DatabaseContext.Entities.DeliveryPoints.ToList();
        DeliveryPoint = DeliveryPoints.First();

        DeliveryTypes = Enum.GetValues(typeof(DeliveryType)).Cast<DeliveryType>();
        DeliveryType = DeliveryTypes.First();

        PaymentMethods = Enum.GetValues(typeof(PaymentMethod)).Cast<PaymentMethod>();
        PaymentMethod = PaymentMethods.First();
    }
}
