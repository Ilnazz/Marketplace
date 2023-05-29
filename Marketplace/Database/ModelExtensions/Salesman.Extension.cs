namespace Marketplace.Database.Models;

public partial class Salesman
{
    public string FullName => $"{Surname} {Name} {Patronymic}";
}
