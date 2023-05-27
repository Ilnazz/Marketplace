using System;
using System.Collections.Generic;

namespace Marketplace.Database.Models;

public partial class Salesman : User
{
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
