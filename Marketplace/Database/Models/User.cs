using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Marketplace.DataTypes.Enums;

namespace Marketplace.Database.Models;

public partial class User
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string? Patronymic { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public byte[]? Photo { get; set; }

    [Column(TypeName = "int")]
    public UserRole Role { get; set; }

    public virtual ICollection<Client> Clients { get; set; } = new List<Client>();

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual ICollection<Salesman> Salesmen { get; set; } = new List<Salesman>();
}
