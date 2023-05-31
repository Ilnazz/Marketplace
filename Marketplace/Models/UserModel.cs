using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Marketplace.Database.Models;

namespace Marketplace.Models;

public partial class UserModel : ObservableObject
{
    #region Properties
    public bool IsAuthorized => App.CurrentUser != null;

    public byte[]? Photo => _user?.Photo;

    public string Name
    {
        get => _user.Name;
        set
        {
            _user.Name = value;
            OnPropertyChanged();
        }
    }

    public string Surname
    {
        get => _user.Surname;
        set
        {
            _user.Surname = value;
            OnPropertyChanged();
        }
    }

    public string? Patronymic
    {
        get => _user.Patronymic;
        set
        {
            _user.Patronymic = value;
            OnPropertyChanged();
        }
    }

    public string FullName => _user.FullName;

    public string Login
    {
        get => _user.Login;
        set
        {
            _user.Login = value;
            OnPropertyChanged();
        }
    }

    public string Password
    {
        get => _user.Password;
        set
        {
            _user.Password = value;
            OnPropertyChanged();
        }
    }
    #endregion

    #region Commands
    [RelayCommand]
    private void Authorize()
    {

    }
    #endregion

    private User _user;

    public UserModel()
    {
        _user = App.CurrentUser;
    }
}
