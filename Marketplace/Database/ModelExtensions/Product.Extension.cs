using System.Linq;

namespace Marketplace.Database.Models;

public partial class Product
{
    public int AvailableQuantity => QuantityInStock - DatabaseContext.Entities.ClientProducts.Local
            .Where(pac => pac.Product == this)
            .Sum(pac => pac.Quantity);

    public bool IsAvailable => AvailableQuantity > 0;

    public bool HasDiscount => DiscountPercent > 0;

    public decimal CostWithDiscount => Cost - Cost * ((decimal)DiscountPercent / 100);
}
