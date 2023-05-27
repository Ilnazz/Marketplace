using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Marketplace.Database;
using Marketplace.Database.Models;

namespace Marketplace.Services;

public static class AuthRegService
{
    public static bool RegisterUser(string name, string surname, string? patronymic, string login, string password)
    {
        if (TryGetUser(login, password, out var _))
            return false;

        return true;
    }

    public static bool AuthorizeUser(string login, string password)
    {
        if (TryGetUser(login, password, out var user) == false)
            return false;
        
        App.CurrentUser = user;
        return true;
    }

    public static bool TryGetUser(string login, string password, [NotNullWhen(true)] out User? user)
    {
        user = DatabaseContext.Entities.Clients.Cast<User>()
            .Concat(DatabaseContext.Entities.Salesmen.Cast<User>())
            .Concat(DatabaseContext.Entities.Employees.Cast<User>())
            .Where(u => u.Login == login && u.Password == password)
            .FirstOrDefault();

        return user != null;
    }
}