using System;
using System.Collections.Generic;

namespace Marketplace.Database.Models;

public partial class Order
{
    public int Id { get; set; }

    public DateTime DateTime { get; set; }

    public int ClientId { get; set; }

    public int OrderStatusId { get; set; }

    public int DeliveryTypeId { get; set; }

    public string? Address { get; set; }

    public int DeliveryPointId { get; set; }

    public virtual Client Client { get; set; } = null!;

    public virtual DeliveryPoint DeliveryPoint { get; set; } = null!;

    public virtual DeliveryType DeliveryType { get; set; } = null!;

    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();

    public virtual OrderStatus OrderStatus { get; set; } = null!;
}
