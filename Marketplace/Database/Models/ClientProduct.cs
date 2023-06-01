using System;
using System.Collections.Generic;

namespace Marketplace.Database.Models;

public partial class ClientProduct
{
    public int ClientId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public virtual Client Client { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
