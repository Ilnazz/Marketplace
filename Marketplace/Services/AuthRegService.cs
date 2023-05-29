using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Marketplace.Database;
using Marketplace.Database.Models;

namespace Marketplace.Services;

public class AuthRegService
{
    public bool TryRegisterUser(string login, string password)
    {
        if (TryGetUser(login, password, out var _))
            return false;

        // Register here

        return true;
    }

    public bool TryAuthorizeUser(string login, string password)
    {
        if (TryGetUser(login, password, out var user))
            App.CurrentUser = user;

        return user != null;
    }

    private bool TryGetUser(string login, string password, [NotNullWhen(true)] out User? user)
    {
        user = DatabaseContext.Entities.Clients.Local.Cast<User>()
            .Concat(DatabaseContext.Entities.Salesmen.Local.Cast<User>())
            .Concat(DatabaseContext.Entities.Employees.Local.Cast<User>())
            .Where(u => u.Login == login && u.Password == password)
            .FirstOrDefault();

        return user != null;
    }
}