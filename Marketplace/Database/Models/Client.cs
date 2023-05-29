using System;
using System.Collections.Generic;

namespace Marketplace.Database.Models;

public partial class Client : User
{
    public virtual ICollection<Basket> Baskets { get; set; } = new List<Basket>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
