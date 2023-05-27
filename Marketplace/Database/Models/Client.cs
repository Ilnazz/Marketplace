using System;
using System.Collections.Generic;

namespace Marketplace.Database.Models;

public partial class Client : User
{
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
