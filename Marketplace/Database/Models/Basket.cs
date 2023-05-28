using System;
using System.Collections.Generic;

namespace Marketplace.Database.Models;

public partial class Basket
{
    public int Id { get; set; }

    public int ClientId { get; set; }

    public virtual ICollection<BasketProduct> BasketProducts { get; set; } = new List<BasketProduct>();

    public virtual Client Client { get; set; } = null!;
}
