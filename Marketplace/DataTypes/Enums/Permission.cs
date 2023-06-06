namespace Marketplace.DataTypes.Enums;

public enum Permission
{
    ViewEditProfile,

    ViewProductsPage,
    ViewProduct,
    AddProduct,
    EditProduct,
    RemoveProduct,

    ViewProductsOnInpection,
    AcceptProduct,
    RejectProduct,

    ViewBasketPage,
    AddRemoveProductToBasket,

    ViewOrdersPage,
    MakeOrder,
    PayForOrder,
    ReceiveOrder,
    CancelOrder,
    EditOrderStatus
}