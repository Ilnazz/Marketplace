﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows.Documents;
using Marketplace.Database;
using Marketplace.Database.Models;
using Marketplace.DataTypes.Enums;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Services;

public class UserService
{
    private const string DefaultUserName = "Новый пользователь";

    private User? _currentUser;
    public User? CurrentUser
    {
        get => _currentUser;
        set
        {
            _currentUser = value;
            StateChanged?.Invoke();
        }
    }

    public event Action? StateChanged;

    public bool TryRegisterUser(string login, string password, UserRole role)
    {
        if (TryGetUser(login, password, out var _))
            return false;

        if (role == UserRole.Client)
        {
            var client = new Client
            {
                Surname = string.Empty,
                Name = DefaultUserName,
                Login = login,
                Password = password,
            };
            DatabaseContext.Entities.Clients.Local.Add(client);
        }
        else if (role == UserRole.Salesman)
        {
            var salesman = new Salesman
            {
                Surname = string.Empty,
                Name = DefaultUserName,
                Login = login,
                Password = password
            };
            DatabaseContext.Entities.Salesmen.Local.Add(salesman);
        }

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
        var client = (Client)CurrentUser;
        var clientBasket = client.Basket;

        // TODO: If you will have extra time, implement: show in client basket, that product was removed or out of stock (disable prod in basket, not count it)
        // Remove products that out of stock or removed from client basket
        var prodAndCountsToRemoveFromClientBasket = new List<ClientProduct>();
        foreach (var prodAndCount in clientBasket)
        {
            if (prodAndCount.Product.QuantityInStock == 0 || prodAndCount.Product.IsRemoved)
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
        if (CurrentUser != null)
            CurrentUser = null;

        App.BasketService = new GuestBasketService<Product>();
    }

    public bool IsUserAuthorized() => CurrentUser != null;

    private bool TryGetUser(string login, string password, [NotNullWhen(true)] out User? user)
    {
        user = DatabaseContext.Entities.Clients.Local.Cast<User>()
            .Concat(DatabaseContext.Entities.Salesmen.Local.Cast<User>())
            .Concat(DatabaseContext.Entities.Employees.Local.Cast<User>())
            .Where(u => u.Login == login && u.Password == password)
            .FirstOrDefault();

        return user != null;
    }

    public UserService()
    {
        DatabaseContext.Entities.Clients.Load();
        DatabaseContext.Entities.Salesmen.Load();
        DatabaseContext.Entities.Employees.Load();
    }
}