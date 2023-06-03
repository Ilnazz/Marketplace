using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Marketplace.DataTypes.Enums;

namespace Marketplace.Database.Models;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public decimal Cost { get; set; }

    public int SalesmanId { get; set; }

    [Column(TypeName = "int")]
    public ProductCategory Category { get; set; }

    public int ManufacturerId { get; set; }

    public int DiscountPercent { get; set; }

    public int QuantityInStock { get; set; }

    [Column(TypeName = "int")]
    public ProductStatus Status { get; set; }

    public virtual ICollection<ClientProduct> ClientProducts { get; set; } = new List<ClientProduct>();

    public virtual ProductManufacturer Manufacturer { get; set; } = null!;

    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();

    public virtual ICollection<ProductPhoto> ProductPhotos { get; set; } = new List<ProductPhoto>();

    public virtual Salesman Salesman { get; set; } = null!;
}
