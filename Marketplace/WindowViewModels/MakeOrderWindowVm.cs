﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.Input;
using Marketplace.Database;
using Marketplace.Database.Models;
using Marketplace.DataTypes.Enums;
using Marketplace.Models;
using Marketplace.WindowViews;
using Wpf.Ui.Controls;

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
            ValidateProperty(value);
            _order.Address = value;
            OnPropertyChanged();
            MakeOrderCommand.NotifyCanExecuteChanged();
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

    public BankCard? BankCard
    {
        get => _order.Client.BankCard;
    }

    public bool CanSetHomeAddress =>
        DeliveryType == DeliveryType.ToHome;

    public bool CanSelectDeliveryPoint =>
        DeliveryType == DeliveryType.ToDeliveryPoint;

    public bool CanAttachBankCard =>
        PaymentMethod == PaymentMethod.ByBankCard;

    public bool IsBankCardAttached =>
        BankCard != null;

    public IEnumerable<ProductModel> ProductModels { get; private set; }
    public int ProductsQuantity => ProductModels.Sum(pm => pm.QuantityInBasket);

    public decimal TotalCost => ProductModels.Sum(pm => pm.TotalCostWithDiscount);
    #endregion

    #region Validation methods
    public static System.ComponentModel.DataAnnotations.ValidationResult ValidateHomeAddress(string? address, ValidationContext context)
    {
        var instance = context.ObjectInstance as MakeOrderWindowVm;
        if (instance!.CanSetHomeAddress && string.IsNullOrWhiteSpace(address))
            return new("Укажите адрес");

        return System.ComponentModel.DataAnnotations.ValidationResult.Success!;
    }
    #endregion

    #region Commands
    [RelayCommand]
    private void ShowBankCardWindow()
    {
        var bankCard = BankCard;
        if (bankCard == null)
        {
            bankCard = new BankCard();
            bankCard.Clients.Add(_order.Client!);
        }

        var bankCardWindowVm = new BankCardWindowVm(bankCard);
        var bankCardWindowView = new BankCardWindowView() { DataContext = bankCardWindowVm };

        var dialogWindow = new Wpf.Ui.Controls.MessageBox
        {
            Content = bankCardWindowView,
            Width = bankCardWindowView.Width + 30,
            Height = bankCardWindowView.Height,
            SizeToContent = SizeToContent.Height,
            ResizeMode = ResizeMode.NoResize,
            Title = bankCardWindowVm.Title,
            ShowFooter = false,
            WindowStartupLocation = WindowStartupLocation.CenterScreen
        };
        bankCardWindowVm.CloseWindowMethod += dialogWindow.Close;
        dialogWindow.ShowDialog();

        OnPropertyChanged(nameof(BankCard));
        OnPropertyChanged(nameof(IsBankCardAttached));
        MakeOrderCommand.NotifyCanExecuteChanged();
    }

    [RelayCommand]
    private void MakeOrder(Flyout messageFlyout)
    {
        ValidateAllProperties();
        MakeOrderCommand.NotifyCanExecuteChanged();

        if (HasErrors)
            return;

        if (PaymentMethod == PaymentMethod.ByBankCard && IsBankCardAttached == false)
        {
            messageFlyout.Content = "Необходимо прикрепить банковскую карту";
            messageFlyout.Show();
            return;
        }

        if (PaymentMethod == PaymentMethod.ByBankCard)
        {
            var dialogWindow = new Wpf.Ui.Controls.MessageBox
            {
                Content = new TextBlock
                {
                    Text = "Оплата банковской картой",
                    FontSize = 18
                },
                SizeToContent = SizeToContent.Height,
                ResizeMode = ResizeMode.NoResize,
                Title = "Оплата",
                WindowStartupLocation = WindowStartupLocation.CenterScreen,

                ButtonLeftName = "Оплатить",
                ButtonLeftAppearance = Wpf.Ui.Common.ControlAppearance.Success,

                ButtonRightName = "Отмена",
            };
            var isRightButtonClicked = false;
            dialogWindow.ButtonRightClick += (_, _) =>
            {
                isRightButtonClicked = true;
                dialogWindow.Close();
            };
            dialogWindow.ButtonLeftClick += (_, _) =>
            {
                dialogWindow.Close();
            };
            dialogWindow.ShowDialog();

            if (isRightButtonClicked == false)
                return;
        }

        foreach (var pm in ProductModels!)
        {
            _order.OrderProducts.Add(new OrderProduct
            {
                Product = pm.Product,
                Quantity = pm.QuantityInBasket,
                Cost = pm.CostWithDiscount
            });
            pm.QuantityInStock -= pm.QuantityInBasket;
        }

        App.BasketService.ClearBasket();

        DatabaseContext.Entities.Orders.Local.Add(_order);
        DatabaseContext.Entities.SaveChanges();

        var okDialogWindow = new Wpf.Ui.Controls.MessageBox
        {
            Content = new TextBlock
            {
                Text = "Заказ оплачен",
                FontSize = 18
            },
            SizeToContent = SizeToContent.Height,
            ResizeMode = ResizeMode.NoResize,
            Title = "Информация",
            WindowStartupLocation = WindowStartupLocation.CenterScreen
        };
        var okButton = new Wpf.Ui.Controls.Button
        {
            Content = "Ок"
        };
        okDialogWindow.Footer = okButton;
        okButton.Click += (_, _) => okDialogWindow.Close();
        okDialogWindow.ShowDialog();

        CloseWindow();
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
