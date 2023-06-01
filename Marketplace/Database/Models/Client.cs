using System;
using System.Collections.Generic;

namespace Marketplace.Database.Models;

public partial class Client : User
{
    public virtual ICollection<ClientProduct> Basket { get; set; } = new List<ClientProduct>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
