using System;
using System.Collections.Generic;

namespace Marketplace.Database.Models;

public partial class Salesman
{
    public int Id { get; set; }

    public int Userid { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual User User { get; set; } = null!;
}
