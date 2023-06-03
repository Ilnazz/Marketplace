using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Marketplace.DataTypes.Enums;

namespace Marketplace.Database.Models;

public partial class Order
{
    public int Id { get; set; }

    public DateTime DateTime { get; set; }

    public int ClientId { get; set; }

    [Column(TypeName = "int")]
    public OrderStatus Status { get; set; }

    [Column(TypeName = "int")]
    public DeliveryType DeliveryType { get; set; }

    public DateTime DeliveryDate { get; set; }

    [Column(TypeName = "int")]
    public PaymentMethod PaymentMethod { get; set; }

    public string? Address { get; set; }

    public int? DeliveryPointId { get; set; }

    public virtual Client Client { get; set; } = null!;

    public virtual DeliveryPoint? DeliveryPoint { get; set; }

    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
}
