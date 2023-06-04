namespace Marketplace.Database.Models;

public partial class Client
{
    public string FullName => User.FullName;
}
