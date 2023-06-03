using System;
using System.Collections.Generic;

namespace Marketplace.Database.Models;

public partial class PaymentMethod
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual Order? Order { get; set; }
}
