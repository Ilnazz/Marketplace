using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Marketplace.Database;
using Marketplace.Database.Models;
using Marketplace.DataTypes.Enums;
using Marketplace.Pages;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Services;

public class UserService
{
    private User _currentUser = null!;
    public User CurrentUser
    {
        get => _currentUser;
        set
        {
            _currentUser = value;

            StateChanged?.Invoke();

            var currentPageTitle = App.NavigationWindowVm?.CurrentPageTitle;
            if (currentPageTitle == null)
                return;

            if ((currentPageTitle.StartsWith("Заказы") &&
                    CurrentUser.Permissions.Contains(Permission.ViewOrdersPage) == false) ||
                (currentPageTitle.StartsWith("Корзина") &&
                    CurrentUser.Permissions.Contains(Permission.ViewBasketPage) == false))
                App.NavigationService.Navigate(typeof(BookProductsPage));
        }
    }

    public event Action? StateChanged;

    private const string DefaultNewUserName = "Новый пользователь";

    private static readonly User _guest = new User { Name = "Гость", Role = UserRole.Guest };

    public bool TryRegisterUser(string login, string password, UserRole role)
    {
        if (TryGetUser(login, password, out var _))
            return false;

        var user = new User
        {
            Surname = string.Empty,
            Name = DefaultNewUserName,
            Login = login,
            Password = password,
            Role = role
        };
        DatabaseContext.Entities.Users.Add(user);

        if (role == UserRole.Client)
            DatabaseContext.Entities.Clients.Local.Add(new Client
            {
                User = user
            });
        else if (role == UserRole.Salesman)
            DatabaseContext.Entities.Salesmen.Local.Add(new Salesman
            {
                User = user
            });
        DatabaseContext.Entities.SaveChanges();

        return true;
    }

    public bool TryAuthorizeUser(string login, string password)
    {
        if (TryGetUser(login, password, out var user) == false)
            return false;

        CurrentUser = user;

        if (CurrentUser.Role != UserRole.Client)
            return true;

        // This is not good in this service, but ...
        var client = user.Client!;
        var clientBasket = client.Basket;

        // TODO: If you will have extra time, implement: show in client basket, that product was removed or out of stock (disable prod in basket, not count it)
        // Remove products that out of stock or removed from client basket
        var prodAndCountsToRemoveFromClientBasket = new List<ClientProduct>();
        foreach (var prodAndCount in clientBasket)
        {
            if (prodAndCount.Product.QuantityInStock == 0 || prodAndCount.Product.Status == ProductStatus.RemovedFromSale)
                prodAndCountsToRemoveFromClientBasket.Add(prodAndCount);
        }
        foreach (var prodAndCount in prodAndCountsToRemoveFromClientBasket)
            clientBasket.Remove(prodAndCount);

        var guestBasket = App.BasketService.GetItemAndCounts();
        foreach ((Product prod, int count) in guestBasket)
        {
            var prodAndCountInClientBasket = clientBasket
                .FirstOrDefault(pac => pac.Product == prod);
            if (prodAndCountInClientBasket != null)
            {
                prodAndCountInClientBasket.Quantity += count;
            }
            else
                clientBasket.Add(new ClientProduct
                {
                    Product = prod,
                    Quantity = count
                });
        }

        DatabaseContext.Entities.SaveChanges();

        App.BasketService = new ClientBasketService(client);

        return true;
    }

    public void LogOutUser()
    {
        if (CurrentUser != _guest)
            CurrentUser = _guest;

        App.BasketService = new GuestBasketService<Product>();
    }

    public bool IsUserAuthorized() => CurrentUser != null;

    public bool IsGuest() => CurrentUser.Role == UserRole.Guest;

    private bool TryGetUser(string login, string password, [NotNullWhen(true)] out User? user)
    {
        user = DatabaseContext.Entities.Users.Local
            .FirstOrDefault(u => u.Login == login && u.Password == password);

        return user != null;
    }

    public UserService()
    {
        CurrentUser = _guest;

        DatabaseContext.Entities.Users.Load();
        DatabaseContext.Entities.Clients.Load();
        DatabaseContext.Entities.Salesmen.Load();
        DatabaseContext.Entities.Employees.Load();
    }
}