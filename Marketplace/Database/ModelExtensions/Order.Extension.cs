﻿using System.Linq;

namespace Marketplace.Database.Models;

public partial class Order
{
    public decimal TotalCost => OrderProducts.Sum(op => op.Cost * op.Quantity);

    public int TotalProductsCount => OrderProducts.Sum(op => op.Quantity);
}