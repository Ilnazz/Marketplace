using System;
using System.Collections.Generic;

namespace Marketplace.Database.Models;

public partial class Employee
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public virtual User User { get; set; } = null!;
}
