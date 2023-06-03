using System.Collections.Generic;
using Marketplace.DataTypes;
using Marketplace.DataTypes.Enums;

namespace Marketplace.Database.Models;

public partial class User
{
    private IEnumerable<Permission> _permissions = null!;
    public IEnumerable<Permission> Permissions => _permissions ??= RolePermissions.All[Role];
}
