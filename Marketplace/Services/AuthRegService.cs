using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Marketplace.Database;
using Marketplace.Database.Models;

namespace Marketplace.Services;

public class AuthRegService
{
    public bool RegisterUser(string name, string surname, string? patronymic, string login, string password)
    {
        if (TryGetUser(login, password, out var _))
            return false;

        // Register here

        return true;
    }

    public User? AuthorizeUser(string login, string password)
    {
        TryGetUser(login, password, out var user);
        return user;
    }

    public bool TryGetUser(string login, string password, [NotNullWhen(true)] out User? user)
    {
        user = DatabaseContext.Entities.Clients.Cast<User>()
            .Concat(DatabaseContext.Entities.Salesmen.Cast<User>())
            .Concat(DatabaseContext.Entities.Employees.Cast<User>())
            .Where(u => u.Login == login && u.Password == password)
            .FirstOrDefault();

        return user != null;
    }
}