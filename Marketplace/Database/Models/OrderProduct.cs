using System.ComponentModel.DataAnnotations.Schema;

namespace Marketplace.Database.Models;

public partial class OrderProduct
{
    public int ReturnedQuantity { get; set; }

    public bool CanReturn => Quantity - ReturnedQuantity > 0 && Order.Status == DataTypes.Enums.OrderStatus.Completed;

    [NotMapped]
    public bool IsReturned => ReturnedQuantity > 0;

    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal Cost { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
