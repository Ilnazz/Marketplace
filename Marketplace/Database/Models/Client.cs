using System;
using System.Collections.Generic;

namespace Marketplace.Database.Models;

public partial class Client
{
    public int Id { get; set; }

    public int? BankCardId { get; set; }

    public int UserId { get; set; }

    public virtual BankCard? BankCard { get; set; }

    public virtual ICollection<ClientProduct> Basket { get; set; } = new List<ClientProduct>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual User User { get; set; } = null!;
}
