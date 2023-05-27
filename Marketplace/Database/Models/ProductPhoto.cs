using System;
using System.Collections.Generic;

namespace Marketplace.Database.Models;

public partial class ProductPhoto
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public byte[] Data { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
