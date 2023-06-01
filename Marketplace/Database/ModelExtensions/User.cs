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

    public string FullName => $"{Surname} {Name} {Patronymic}";
    #endregion
}
