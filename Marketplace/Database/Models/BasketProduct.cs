using System;
using System.Collections.Generic;

namespace Marketplace.Database.Models;

public partial class BasketProduct
{
    public int BasketId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public virtual Basket Basket { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
