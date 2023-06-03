using System;
using System.Collections.Generic;

namespace Marketplace.Database.Models;

public partial class BankCard
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Number { get; set; } = null!;

    public DateTime ExpirationDate { get; set; }

    public short ValidationCode { get; set; }

    public virtual ICollection<Client> Clients { get; set; } = new List<Client>();
}
