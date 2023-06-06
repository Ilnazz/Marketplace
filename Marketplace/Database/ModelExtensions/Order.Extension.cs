using System.Linq;
using Marketplace.DataTypes.Enums;

namespace Marketplace.Database.Models;

public partial class Order
{
    public decimal TotalCost => OrderProducts.Sum(op => op.Cost * op.Quantity);

    public int TotalProductsCount => OrderProducts.Sum(op => op.Quantity);

    public bool RequiresPay =>
        PaymentMethod == PaymentMethod.InCash && Status == OrderStatus.Delivered;
}
