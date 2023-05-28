using System;
using System.Collections.Generic;

namespace Marketplace.Database.Models;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public decimal Cost { get; set; }

    public int SalesmanId { get; set; }

    public int ProductCategoryId { get; set; }

    public int ProductManufacturerId { get; set; }

    public virtual ICollection<BasketProduct> BasketProducts { get; set; } = new List<BasketProduct>();

    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();

    public virtual ProductCategory ProductCategory { get; set; } = null!;

    public virtual ProductManufacturer ProductManufacturer { get; set; } = null!;

    public virtual ICollection<ProductPhoto> ProductPhotos { get; set; } = new List<ProductPhoto>();

    public virtual Salesman Salesman { get; set; } = null!;
}
