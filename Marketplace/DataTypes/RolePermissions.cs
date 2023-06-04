﻿using System.Collections.Generic;
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

                Permission.ViewBasketPage,
                Permission.AddRemoveProductToBasket,
            }
        },
        {
            UserRole.Client,
            new[] {
                Permission.ViewEditProfile,

                Permission.ViewProductsPage,
                Permission.ViewProduct,

                Permission.ViewBasketPage,
                Permission.AddRemoveProductToBasket,

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
                Permission.ViewEditProfile,

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
                Permission.ViewEditProfile,

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