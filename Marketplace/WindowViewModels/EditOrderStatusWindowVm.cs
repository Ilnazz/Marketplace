using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Marketplace.Database;
using Marketplace.Database.Models;
using Marketplace.DataTypes.Enums;
using Marketplace.WindowViews;

namespace Marketplace.WindowViewModels;

public partial class EditOrderStatusWindowVm : WindowVmBase
{
    public Order Order { get; init; }

    public IEnumerable<OrderStatus> OrderStatuses { get; init; }

    public OrderStatus SelectedStatus
    {
        get => Order.Status;
        set
        {
            Order.Status = value;
            OnPropertyChanged();
        }
    }

    [RelayCommand]
    private void Ok()
    {
        DatabaseContext.Entities.SaveChanges();
        CloseWindow();
    }

    public EditOrderStatusWindowVm(Order order)
    {
        Title = "Редактирование статуса заказа";
        Order = order;

        OrderStatuses = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>().Skip(1);
    }
}
