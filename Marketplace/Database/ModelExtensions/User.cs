using System.Collections;
using System.Collections.Generic;
using Marketplace.DataTypes;
using Marketplace.DataTypes.Enums;

namespace Marketplace.Database.Models;

public class User
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string? Patronymic { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public byte[]? Photo { get; set; } = null!;

    #region Extensions
    public UserRole Role
    {
        get
        {
            if (this is Client)
                return UserRole.Client;
            else if (this is Salesman)
                return UserRole.Salesman;

            return UserRole.Employee;
        }
    }

    private IEnumerable<Permission> _permissions = null!;
    public IEnumerable<Permission> Permissions => _permissions ??= RolePermissions.All[Role];

    public Client Client => (Client)this;
    public Salesman Salesman => (Salesman)this;
    public Employee Employee => (Employee)this;

    public string FullName => $"{Surname} {Name} {Patronymic}";
    #endregion
}
