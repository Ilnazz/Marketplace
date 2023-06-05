using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.Input;
using Marketplace.Database;
using Marketplace.Database.Models;
using Marketplace.DataTypes.Enums;
using Marketplace.DataTypes.Records;
using Marketplace.Models;
using Marketplace.Pages;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.PageViewModels;

public partial class OrdersPageVm : PageVmBase
{
    #region Properties
    public IEnumerable<Order> Orders => GetFilteredAndSortedOrders();

    public bool AreThereOrders => Orders.Any();

    public IEnumerable<Sorting<Order>> Sortings { get; init; } = new[]
    {
        new Sorting<Order>("По умолчанию", null! ),

        new Sorting<Order>("По дате заказа ↑", o => o.OrderBy(o => o.DateTime) ),
        new Sorting<Order>("По дате заказа ↓", o => o.OrderByDescending(o => o.DateTime) ),

        new Sorting<Order>("По дате доставке ↑", o => o.OrderBy(o => o.DeliveryDate) ),
        new Sorting<Order>("По дате доставке ↓", o => o.OrderByDescending(o => o.DeliveryDate) ),

        new Sorting<Order>("По количеству продуктов ↑", o => o.OrderBy(o => o.OrderProducts.Sum(op => op.Quantity)) ),
        new Sorting<Order>("По количеству продуктов ↓", o => o.OrderByDescending(o => o.OrderProducts.Sum(op => op.Quantity)) ),
    };

    private Sorting<Order> _slectedSorting = null!;
    public Sorting<Order> SelectedSorting
    {
        get => _slectedSorting;
        set
        {
            _slectedSorting = value;

            OnPropertyChanged();
            OnPropertyChanged(nameof(Orders));
            OnPropertyChanged(nameof(AreThereOrders));
        }
    }

    public IEnumerable<OrderStatus> Statuses { get; init; }

    private OrderStatus _selectedStatus;
    public OrderStatus SelectedStatus
    {
        get => _selectedStatus;
        set
        {
            _selectedStatus = value;

            OnPropertyChanged();
            OnPropertyChanged(nameof(Orders));
            OnPropertyChanged(nameof(AreThereOrders));
        }
    }

    public IEnumerable<PaymentMethod> PaymentMethods { get; init; }

    private PaymentMethod _selectedPaymentMethod;
    public PaymentMethod SelectedPaymentMethod
    {
        get => _selectedPaymentMethod;
        set
        {
            _selectedPaymentMethod = value;

            OnPropertyChanged();
            OnPropertyChanged(nameof(Orders));
            OnPropertyChanged(nameof(AreThereOrders));
        }
    }

    public IEnumerable<DeliveryType> DeliveryTypes { get; init; }

    private DeliveryType _selectedDeliveryType;
    public DeliveryType SelectedDeliveryType
    {
        get => _selectedDeliveryType;
        set
        {
            _selectedDeliveryType = value;

            OnPropertyChanged();
            OnPropertyChanged(nameof(Orders));
            OnPropertyChanged(nameof(AreThereOrders));
        }
    }
    #endregion

    #region Commands
    [RelayCommand]
    private void NavigateToProductsPage() =>
        App.NavigationService.Navigate(typeof(BookProductsPage));
    #endregion

    private readonly IEnumerable<Order> _allOrders = null!;

    public OrdersPageVm()
    {
        DatabaseContext.Entities.Orders.Load();

        _allOrders = GetAllOrders();

        SelectedSorting = Sortings.First();

        Statuses = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>();
        SelectedStatus = Statuses.First();

        PaymentMethods = Enum.GetValues(typeof(PaymentMethod)).Cast<PaymentMethod>();
        SelectedPaymentMethod = PaymentMethods.First();

        DeliveryTypes = Enum.GetValues(typeof(DeliveryType)).Cast<DeliveryType>();
        SelectedDeliveryType = DeliveryTypes.First();

        App.SearchService.IsEnabled = false;
    }

    #region Private methods
    private IEnumerable<Order> GetAllOrders()
    {
        var user = App.UserService.CurrentUser;
        if (user.Role == DataTypes.Enums.UserRole.Client)
            return user.Client!.Orders;
        else if (user.Role == DataTypes.Enums.UserRole.Salesman)
            return DatabaseContext.Entities.Orders.Local
                .SelectMany(o => o.OrderProducts)
                .Where(op => user.Salesman!.Products.Any(p => p == op.Product))
                .Select(op => op.Order);
        else if (user.Role == DataTypes.Enums.UserRole.Employee)
            return DatabaseContext.Entities.Orders.Local.ToList();

        throw new Exception("Guest does not have orders");
    }

    private IEnumerable<Order> GetFilteredAndSortedOrders()
    {
        var filtered = _allOrders.Where(OrderFilter);
        var sorted = SortOrders(filtered);
        return sorted;
    }

    private IEnumerable<Order> SortOrders(IEnumerable<Order> orders)
        => SelectedSorting.Sorter != null ? SelectedSorting.Sorter(orders) : orders;

    private bool OrderFilter(Order order) =>
        StatusFilter(order) &&
        PaymentMethodFilter(order) &&
        DeliveryTypeFilter(order);

    private bool StatusFilter(Order order)
        => SelectedStatus == OrderStatus.None || order.Status == SelectedStatus;

    private bool PaymentMethodFilter(Order order)
        => SelectedPaymentMethod == PaymentMethod.None || order.PaymentMethod == SelectedPaymentMethod;

    private bool DeliveryTypeFilter(Order order)
        => SelectedDeliveryType == DeliveryType.None || order.DeliveryType == SelectedDeliveryType;
    #endregion
}