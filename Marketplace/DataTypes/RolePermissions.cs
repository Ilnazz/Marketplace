using System.Collections.Generic;
using Marketplace.DataTypes.Enums;

namespace Marketplace.DataTypes;

public static class RolePermissions
{
    public static readonly IReadOnlyDictionary<UserRole, IEnumerable<Permission>> All = new Dictionary<UserRole, IEnumerable<Permission>>()
    {
        {
            UserRole.Guest,
            new[] {
                Permission.ViewProductsPage,
                Permission.ViewProduct,

                Permission.ViewBasket,
                Permission.AddProductToBasket,
                Permission.RemoveProductFromBasket
            }
        },
        {
            UserRole.Client,
            new[] {
                Permission.ViewProfile,
                Permission.EditProfile,

                Permission.ViewProductsPage,
                Permission.ViewProduct,

                Permission.ViewBasket,
                Permission.AddProductToBasket,
                Permission.RemoveProductFromBasket,

                Permission.MakeOrder,
                Permission.PayForOrder,
                Permission.CancelOrder,
                Permission.ViewOrdersPage,
                Permission.ViewOrder
            }
        },
        {
            UserRole.Salesman,
            new[]
            {
                Permission.ViewProfile,
                Permission.EditProfile,

                Permission.ViewProductsPage,
                Permission.ViewProduct,
                Permission.AddProduct,
                Permission.EditProduct,
                Permission.RemoveProduct,

                Permission.ViewOrdersPage,
                Permission.ViewOrder
            }
        },
        {
            UserRole.Employee,
            new[]
            {
                Permission.ViewProfile,
                Permission.EditProfile,

                Permission.ViewProductsPage,
                Permission.ViewProduct,
                Permission.ViewProductsOnInpection,
                Permission.AcceptProduct,
                Permission.RejectProduct,

                Permission.ViewOrdersPage,
                Permission.ViewOrder
            }
        }
    };
}