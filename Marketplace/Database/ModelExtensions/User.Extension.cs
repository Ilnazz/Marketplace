using System.Collections.Generic;
using System.Linq;
using Marketplace.DataTypes;
using Marketplace.DataTypes.Enums;

namespace Marketplace.Database.Models;

public partial class User
{
    private IEnumerable<Permission> _permissions = null!;
    public IEnumerable<Permission> Permissions => _permissions ??= RolePermissions.All[Role];

    public Client? Client => Clients.FirstOrDefault();
    public Salesman? Salesman => Salesmen.FirstOrDefault();
    public Employee? Employee => Employees.FirstOrDefault();

    public string FullName => $"{Surname} {Name} {Patronymic}";
}
