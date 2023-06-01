using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Marketplace.Database;
using Marketplace.Database.Models;
using Marketplace.DataTypes.Enums;

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
        if (TryGetUser(login, password, out var user))
            CurrentUser = user;

        return user != null;
    }

    public void LogOutUser()
    {
        if (CurrentUser != null)
            CurrentUser = null;
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
}